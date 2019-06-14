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
            //transform course view model to entity
            Course courses = new Course { Id = course.Id, AmountOfStudents = course.AmountOfStudents ,DurationDays=course.DurationDays,
                Topic = new Topic { TopicName = course.Topic }, CourseName=course.CourseName,StartDate=course.StartDate};
            //if(string.IsNullOrEmpty(course.CourseName))
            //    ModelState.AddModelError("CourseName", "Введите свое имя");
            //Checkig for validation errors
            if (ModelState.IsValid)
           {
                courses.Topic.Id = Id;
                UO.Courses.Update(courses);
                TempData["message"] = string.Format("Changes in course \"{0}\" was saved", course.CourseName);
                return RedirectToAction("Index");
            }
           else
                ////In case that there are val. errors sent back data
           //{
           //     CourseViewModel courseVM = new CourseViewModel
           //     {
                    
           //         Id = Id,
           //         AmountOfStudents = courses.AmountOfStudents,
           //         CourseName = courses.CourseName,
           //         DurationDays = courses.DurationDays,
           //         StartDate = courses.StartDate,
           //         Topic = courses.Topic.TopicName
           //     };

                return View(course);
            //}
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
    }
}
