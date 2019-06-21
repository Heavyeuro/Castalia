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
    [Authorize(Roles = "teacher")]
    public class TeacherController : Controller
    {
        IUnitOfWork UO;
        public int PageSize = 2;

        public TeacherController(IUnitOfWork repo)
        {
            UO = repo;
        }

        //GET
        public ViewResult Index(string currCourse, int page = 1)
        {
            string teacherName = UO.NickName.GetAll()
                .Where(m=>m.UserName==HttpContext.User.Identity.Name)
                .First().Teacher.TeacherName;

            //chech for validation error
            if (TempData["CustomError"] != null)
                ModelState.AddModelError("Mark", TempData["CustomError"].ToString());

            LearnerViewModel learnerView = new LearnerViewModel()
            {
                CoursesList = new List<string>(),
                logs = new List<Log>(),
                CurrentCourse = currCourse,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                }
            };

            //select course name according current name
            if (currCourse == null)
                learnerView.CurrentCourse = UO.Courses.GetAll().Where(x => x.Teacher.TeacherName == teacherName).First().CourseName;

            foreach (var log in UO.Logs.GetAll().Where(x => x.Course.CourseName == learnerView.CurrentCourse))
                learnerView.logs.Add(log);
            learnerView.PagingInfo.TotalItems = learnerView.logs.Count();

            var courses = UO.Courses.GetAll();
            foreach (var course in courses)
                if ((course.Teacher != null) && (course.Teacher.TeacherName == teacherName))
                    learnerView.CoursesList.Add(course.CourseName);
            
            //selecting logs according page context
            learnerView.logs = learnerView.logs.Skip((page - 1) * PageSize)
                    .Take(PageSize).ToList();

            return View(learnerView);
        }

        [HttpPost]
        public ActionResult AddRate(int LogId, int Mark)
        {

            //Checkig for validation errors
            if (Mark > 100 || Mark < 0)
            {
                TempData["CustomError"] = "Mark shoyld be in range from 0 to 100";
                ModelState.AddModelError("","");
            }
            
            //save mark if it is valid
            if (ModelState.IsValid)
            {
                var log = UO.Logs.Get(LogId);
                log.Mark = Mark;
                UO.Save();
                TempData["message"] = string.Format("Student {0} was rated in course \"{1}\"", log.Lerner.LearnerName, log.Course.CourseName);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// in AJAX method display all available teachers
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult TeacherListPartial (int Id)
        {
            ViewBag.Id = Id;

            return PartialView(UO.Teachers.GetAll().ToList());
        }
    }
}