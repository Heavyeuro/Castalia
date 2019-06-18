using Castalia.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Castalia.WEB.Models;
using Castalia.Domain.Entities;

namespace Castalia.WEB.Controllers
{
    public class StudentController : Controller
    {
        IUnitOfWork UO;
        public int PageSize = 3;

        public StudentController(IUnitOfWork repo)
        {
            UO = repo;
        }


        public ActionResult StudentsCourses(string courseStatus, int page = 1)
        {

            string learnerName = HttpContext.User.Identity.Name;
            //learnerName = "Ivanov Ivan";
            LogViewModel logModel = new LogViewModel()
            {
                CourseStatus = courseStatus,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                }

            };
            switch (logModel.CourseStatus)
            {
                case "notStarted":
                    logModel.Logs = UO.Logs.GetAll().Where(x => x.Mark != null && x.Lerner.LearnerName == learnerName
                    && x.Course.StartDate > DateTime.Now).Skip((page - 1) * PageSize).Take(PageSize).ToList();
                    break;
                case "finished":
                    logModel.Logs = UO.Logs.GetAll().Where(x => x.Mark != null && x.Lerner.LearnerName == learnerName
                    && x.Course.StartDate.AddDays(x.Course.DurationDays) < DateTime.Now).Skip((page - 1) * PageSize).Take(PageSize).ToList();
                    break;
                case "inProgress":
                    logModel.Logs = UO.Logs.GetAll().Where(x => x.Mark != null && x.Lerner.LearnerName == learnerName
                    && x.Course.StartDate < DateTime.Now && x.Course.StartDate.AddDays(x.Course.DurationDays) > DateTime.Now)
                    .Skip((page - 1) * PageSize).Take(PageSize).ToList();
                    break;
            }
            List<Log> logs = UO.Logs.GetAll().Where(x => x.Mark != null && x.Lerner.LearnerName == learnerName
                    && x.Course.StartDate.AddDays(x.Course.DurationDays) < DateTime.Now).Skip((page - 1) * PageSize).Take(PageSize).ToList();

            logModel.PagingInfo.TotalItems = logModel.Logs.Count();

            return View(logModel);
        }

        public ActionResult StudentNav()
        {
            return PartialView();
        }

        public ActionResult Register(int Id)
        {
            string currentStudent = HttpContext.User.Identity.Name;
            //temp
            currentStudent = "Ivanov Ivan";
            Course course = UO.Courses.Get(Id);
            if (course.StartDate > DateTime.Now)
            {
                Log log = new Log()
                {
                    RegisterDate = DateTime.Now,
                    Lerner = UO.Learners.GetAll().Where(x => x.LearnerName == currentStudent).First(),
                    Course = course
                };
                UO.Logs.Create(log);
                course.AmountOfStudents++;
                UO.Save();
                TempData["message"] = string.Format("Successful registration on course\"{0}\" !", course.CourseName);
            }
            else
                TempData["message"] = string.Format("Unfortunatly course \"{0}\" isn't available for registration", course.CourseName);



            return RedirectToAction("SelectionByTopic", "Home", null);
        }
    }
}