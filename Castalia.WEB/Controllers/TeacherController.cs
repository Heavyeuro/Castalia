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
    public class TeacherController : Controller
    {
        IUnitOfWork UO;
        public int PageSize = 2;

        public TeacherController(IUnitOfWork repo)
        {
            UO = repo;
        }

        public ViewResult Index(string currCourse, int page = 1)
        {
            string teacherName = HttpContext.User.Identity.Name;
            teacherName = "Nikolev Oleksei";

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

            if (currCourse == null)
                learnerView.CurrentCourse = UO.Courses.GetAll().Where(x => x.Teacher.TeacherName == teacherName).First().CourseName;

            foreach (var log in UO.Logs.GetAll().Where(x => x.Course.CourseName == learnerView.CurrentCourse))
                learnerView.logs.Add(log);
            learnerView.PagingInfo.TotalItems = learnerView.logs.Count();
            var courses = UO.Courses.GetAll();
            foreach (var course in courses)
            {
                if (course.Teacher != null && course.Teacher.TeacherName == teacherName)
                {
                    learnerView.CoursesList.Add(course.CourseName);
                }
            }
            learnerView.logs = learnerView.logs.Skip((page - 1) * PageSize)
                    .Take(PageSize).ToList();
            return View(learnerView);
        }

        [HttpPost]
        public ActionResult AddRate(int LogId, int Mark)
        {

            if (Mark > 100 || Mark < 0)
                ModelState.AddModelError("Mark", "Mark shoyld be in range from 0 to 100");
            //Checkig for validation errors
            if (ModelState.IsValid)
            {
                UO.Logs.Get(LogId).Mark = Mark;
                UO.Save();
            }
            return RedirectToAction("Index");
        }
    }
}