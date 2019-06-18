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
            Course course= UO.Courses.GetAll().Where(i => i.Id == Id).First();
            CourseViewModel courseVM = new CourseViewModel { Id=Id,AmountOfStudents=course.AmountOfStudents,CourseName=course.CourseName,
                DurationDays =course.DurationDays,StartDate=course.StartDate,Topic=course.Topic.TopicName};
            //creating course model for view
            return View(courseVM);
        }

        [HttpPost]
        public ActionResult Edit(CourseViewModel course,int Id)
        {
            //if(string.IsNullOrEmpty(course.CourseName))
            //    ModelState.AddModelError("CourseName", "Введите свое имя");
            //Checkig for validation errors
            if (ModelState.IsValid)
           {
                Course courses = UO.Courses.Get(course.Id);
                courses.DurationDays = course.DurationDays;
                courses.CourseName = course.CourseName;
                courses.StartDate = course.StartDate;

                int i = UO.Topics.GetAll().Where(x => x.TopicName == course.Topic).Count();
                if (i > 0) courses.Topic = UO.Topics.Get(UO.Topics.GetAll().Where(x => x.TopicName == course.Topic).First().Id);
                else courses.Topic = new Topic { TopicName = course.Topic };
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


        [HttpPost]
        public ActionResult Delete( int Id)
        {
            string deletedCourse = UO.Courses.Get(Id).CourseName;
                UO.Courses.Delete(Id);

                TempData["message"] = string.Format("Course \"{0}\" was deleted", deletedCourse);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult LearnerList (int page=1)
        {
            LearnerListViewModel learnerList = new LearnerListViewModel()
            {
                Learners = UO.Learners.GetAll().Skip((page - 1) * PageSize)
                   .Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = UO.Learners.GetAll().Count()
                }
            };
            
            return View(learnerList);
        }

        [HttpPost]
        public ActionResult LearnerList( IEnumerable<Learner> learners)
        {
            return View(UO.Learners.GetAll());
        }

        public ActionResult ManagingStudents(int Id,int page)
        {
            UO.Learners.Get(Id).IsBlocked = !UO.Learners.Get(Id).IsBlocked;
            UO.Save();
            LearnerListViewModel learnerList = new LearnerListViewModel()
            {
                Learners = UO.Learners.GetAll().Skip((page - 1) * PageSize)
                   .Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = UO.Learners.GetAll().Count()
                }
            };
            return View("LearnerList", learnerList);
        }

        public ActionResult AddTeacher(string teacherName)
        {
            UO.Teachers.Create(new Teacher() { TeacherName = teacherName });
            UO.Save();
            return View();//same as in index
        }

        public ActionResult TeacherView(int page =1)
        {

            CourseListViewModel courseList = new CourseListViewModel()
            {
                Courses = new List<Course>(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize
                }
            };
            courseList.Teachers = UO.Teachers.GetAll().ToList();
            foreach(var course in UO.Courses.GetAll())
                if (course.Teacher == null)
                {
                    courseList.PagingInfo.TotalItems++;
                    courseList.Courses.Add(course);
                }

            return View(courseList);
        }

        public ActionResult AppointTeacher(int id, int teacherId)
        {

            UO.Courses.Get(id).Teacher = UO.Teachers.Get(teacherId);
            UO.Save();

            CourseListViewModel courseList = new CourseListViewModel()
            {
                Courses = new List<Course>(),
                PagingInfo = new PagingInfo
                {

                    ItemsPerPage = PageSize
                }
            };

            foreach (var course in UO.Courses.GetAll())
                if (course.Teacher == null)
                {
                    courseList.PagingInfo.TotalItems++;
                    courseList.Courses.Add(course);
                }

            return View("TeacherView", courseList);
        }

        public ActionResult AdminNav()
        {
            return PartialView();
        }


    }
}
