using Castalia.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Castalia.WEB.Models;
using Castalia.Domain.Entities;
using Castalia.WEB.Infrastructure;

namespace Castalia.WEB.Controllers
{
    [Authorize(Roles = "user")]
    public class StudentController : Controller
    {
        IUnitOfWork UO;
        public int PageSize = 3;

        public StudentController(IUnitOfWork repo)
        {
            UO = repo;
        }

        //GET 
        [NotConfirmedNameException]
        public ActionResult StudentsCourses(string courseStatus, int page = 1)
        {
            var a = UO.NickName.GetAll()
                   .Where(m => m.UserName == HttpContext.User.Identity.Name)
                   .FirstOrDefault() ?? throw new NotConfirmedNameException(HttpContext.User.Identity.Name);
            string learnerName 
                = UO.NickName.GetAll()
                .Where(m => m.UserName == HttpContext.User.Identity.Name)
                .FirstOrDefault().Learner.LearnerName;

            //initializing view model for exact learner 
            LogViewModel logModel = new LogViewModel()
            {
                CourseStatus = courseStatus,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                }
            };
            //sorting courses according date
            logModel.Logs = SortLogs(logModel.CourseStatus,learnerName)
                .Skip((page - 1) * PageSize).Take(PageSize).ToList();
            
   
            logModel.PagingInfo.TotalItems = logModel.Logs.Count();
            return View(logModel);
        }

        //POST
        [BlockedStudentException]
        public ActionResult Register(int Id)
        {

            var currentStudent = UO.NickName.GetAll()
                .Where(m => m.UserName == HttpContext.User.Identity.Name)
                .FirstOrDefault().Learner;

            //In case if student is blocked deny access for him
            if (currentStudent.IsBlocked)
                throw new BlockedStudentException(currentStudent.LearnerName); 
            
            //initializing model for registration
            Course course = UO.Courses.Get(Id);
            if (course.StartDate > DateTime.Now )
            {
                Log log = new Log()
                {
                    RegisterDate = DateTime.Now,
                    Lerner = UO.Learners.GetAll().Where(x => x.LearnerName == currentStudent.LearnerName).First(),
                    Course = course
                };
                UO.Logs.Create(log);
                //adding 1 to amount of students in course
                course.AmountOfStudents++;
                UO.Save();
                TempData["message"] = string.Format("Successful registration on course\"{0}\" !", course.CourseName);
            }
            else
                // If we got this far, something failed, redisplay form
                TempData["message"] = string.Format("Unfortunatly course \"{0}\" isn't available for registration", course.CourseName);

            return RedirectToAction("SelectionByTopic", "Home", null);
        }


        public List<Log> SortLogs(string CourseStatus, string learnerName)
        {
            List<Log> logs=new List<Log>(); 
            switch (CourseStatus)
            {
                case "notStarted":
                    logs = UO.Logs.GetAll().Where(x => (x.Lerner.LearnerName == learnerName)
                    && x.Course.StartDate > DateTime.Now).ToList();
                    break;
                case "finished":
                    logs = UO.Logs.GetAll().Where(x => x.Lerner.LearnerName == learnerName
                    && x.Course.StartDate.AddDays(x.Course.DurationDays) < DateTime.Now).ToList();
                    break;
                case "inProgress":
                    logs = UO.Logs.GetAll().Where(x=> x.Lerner.LearnerName == learnerName
                    && (x.Course.StartDate<DateTime.Now) &&(x.Course.StartDate.AddDays(x.Course.DurationDays)) > DateTime.Now).ToList();
                    break;
            }
            return logs;
        }
}
}