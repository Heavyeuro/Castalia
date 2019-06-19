using Castalia.Domain.Entities;
using Castalia.Domain.Interfaces;
using Castalia.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Castalia.WEB.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        IUnitOfWork UO;
        public int PageSize = 3;

        public AdminController(IUnitOfWork repo)
        {
            UO = repo;
        }

        public ViewResult Index()
        {
            return View(UO.Courses.GetAll());
        }

        [HttpGet]
        public ViewResult Edit(int Id)
        {
            Course course = UO.Courses.GetAll().Where(i => i.Id == Id).First();
            //creating course model for view
            CourseViewModel courseVM = new CourseViewModel
            {
                Id = Id,
                AmountOfStudents = course.AmountOfStudents,
                CourseName = course.CourseName,
                DurationDays = course.DurationDays,
                StartDate = course.StartDate,
                Topic = course.Topic.TopicName
            };
            return View(courseVM);
        }

        [HttpPost]
        public ActionResult Edit(CourseViewModel course, int Id)
        {
            //if(string.IsNullOrEmpty(course.CourseName))
            //    ModelState.AddModelError("CourseName", "Введите свое имя");
            //Checkig for validation errors
            if (ModelState.IsValid)
            {
                Course courses;
                if (Id != 0)
                   courses = UO.Courses.Get(course.Id);
                else  courses = new Course();

                courses.DurationDays = course.DurationDays;
                courses.CourseName = course.CourseName;
                courses.StartDate = course.StartDate;
                

                if (UO.Topics.GetAll().Where(x => x.TopicName == course.Topic).Count() > 0)
                    courses.Topic = UO.Topics.Get(UO.Topics.GetAll().Where(x => x.TopicName == course.Topic).First().Id);
                else courses.Topic = new Topic { TopicName = course.Topic };
                UO.Courses.Update(courses);
                UO.Save();

                TempData["message"] = string.Format("Changes in course \"{0}\" was saved", course.CourseName);
                return RedirectToAction("Index");
            }
            else
                return View(course);
        }

        public ViewResult Create()
        {
            return View("Edit", new CourseViewModel());
        }


        public ActionResult Delete(int Id)
        {
            string deletedCourse = UO.Courses.Get(Id).CourseName;
            UO.Courses.Delete(Id);

            TempData["message"] = string.Format("Course \"{0}\" was deleted", deletedCourse);

             return View("Index",UO.Courses.GetAll());
        }

        [HttpGet]
        public ActionResult LearnerList(int page = 1)
        {
            return View( new LearnerListViewModel()
            {
                Learners = UO.Learners.GetAll().Skip((page - 1) * PageSize)
                   .Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = UO.Learners.GetAll().Count()
                }
            });
        }

        public ActionResult ManagingStudents(int Id, int page=1)
        {
            //reversing value of isClocked of the exact learner
            UO.Learners.Get(Id).IsBlocked = !UO.Learners.Get(Id).IsBlocked;
            UO.Save();
            return RedirectToAction("LearnerList");
        }

        [HttpPost]
        public ActionResult AddTeacher(string teacherName)
        {
            if (String.IsNullOrEmpty(teacherName))
                TempData["CustomError"] = "Please enter teacher name";
                
            if (UO.Teachers.GetAll().Where(x => x.TeacherName == teacherName).FirstOrDefault() != null)
                TempData["CustomError"] = "You can`t add existing teacher";

            if (teacherName.All(x => !(char.IsLetter(x) || char.IsWhiteSpace(x)||x=='-')))
                TempData["CustomError"] = "Unacceptable symbols detected";
            
            if (TempData["CustomError"]==null)
            {
                UO.Teachers.Create(new Teacher() { TeacherName = teacherName });
                UO.Save();
                TempData["message"] = string.Format("Teacher \"{0}\" was added", teacherName);
            }
            return RedirectToAction("TeacherAppointView");
        }

        public ActionResult TeacherAppointView(int page = 1)
        {
            if (TempData["CustomError"] != null)
                ModelState.AddModelError("", TempData["CustomError"].ToString());
            
            CourseListViewModel courseList = new CourseListViewModel()
            {
                Teachers = UO.Teachers.GetAll().ToList(),
                Courses = UO.Courses.GetAll().Where(x => x.Teacher == null).ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize
                }
            };
            //foreach (var course in UO.Courses.GetAll())
            //    if (course.Teacher == null)
            //        courseList.Courses.Add(course);

            courseList.PagingInfo.TotalItems= courseList.Courses.Count();

            return View(courseList);
        }

        public ActionResult AppointTeacher(int id, int teacherId)
        {
            UO.Courses.Get(id).Teacher = UO.Teachers.Get(teacherId);
            UO.Save();
            return RedirectToAction("TeacherAppointView");
        }
    }
}
