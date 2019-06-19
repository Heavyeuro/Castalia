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

        public ViewResult Index(string currCourse, int page = 1)
        {
           
            string teacherName = UO.NickName.GetAll()
                .Where(m=>m.UserName==HttpContext.User.Identity.Name)
                .First().Teacher.TeacherName;
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("Mark", TempData["CustomError"].ToString());
            }


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
            {
                TempData["CustomError"] = "Mark shoyld be in range from 0 to 100";
                ModelState.AddModelError("","");
            }
            //Checkig for validation errors
            if (ModelState.IsValid)
            {
                UO.Logs.Get(LogId).Mark = Mark;
                UO.Save();
            }
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "admin")]
        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult TeacherListPartial (int Id)
        {
            ViewBag.Id = Id;
            List<Teacher> teachers = new List<Teacher>();
            teachers = UO.Teachers.GetAll().ToList();
            return PartialView(teachers);
        }
    }
}