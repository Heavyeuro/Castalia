using Castalia.Domain.Entities;
using Castalia.Domain.Interfaces;
using Castalia.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Castalia.WEB.Controllers
{
    public class HomeController : Controller
    {

        private IUnitOfWork UO;
        public int PageSize = 3;

        public HomeController(IUnitOfWork repo)
        {
            UO = repo;
        }

        /// <summary>
        /// Creates selection of all existed in database names of topics
        /// </summary>
        public ViewResult SelectionByTopic(string sortingParam, string sortOrder, int page = 1)
        {
            var courses = UO.Courses.GetAll()
                   .Where(p => sortingParam == null || p.Topic.TopicName == sortingParam);

            int amountOfItems = courses.Count();

            CourseListViewModel model = CourseListInitializer( sortOrder, sortingParam, page);
            model.PagingInfo.TotalItems=amountOfItems;
            ViewBag.sortingParam = sortingParam;

            model.Courses = SortingCourses(courses, sortOrder)
                   .Skip((page - 1) * PageSize)
                   .Take(PageSize).ToList();

            if (User.IsInRole("user"))
                model.StudentRefisterPosibility = StudentRefisterPosibility(model.Courses);

            return View("TopicView", model);
        }

        /// <summary>
        /// Creates selection of all existed in database names of teachers
        /// </summary>
        public ViewResult SelectionByTeacher(string sortingParam, string sortOrder, int page = 1)
        {

            ViewBag.sortingParam = sortingParam;

            CourseListViewModel model = CourseListInitializer( sortOrder, sortingParam, page);

            model.Courses = SortingCourses(GetCourses(sortingParam), sortOrder).ToList();

            model.PagingInfo.TotalItems = model.Courses.Count();

            model.Courses = model.Courses.Skip((page - 1) * PageSize)
                    .Take(PageSize).ToList();

            if (User.IsInRole("user"))
                model.StudentRefisterPosibility = StudentRefisterPosibility(model.Courses);

            return View("TeacherView", model);
        }


        public ActionResult Index()
        {
            return RedirectToAction("SelectionByTopic");
        }

        public CourseListViewModel CourseListInitializer( string sortOrder, string sortingParam, int page = 1)
        {

            Dictionary<string, string> sortedParam = new Dictionary<string, string>(3);
            sortedParam.Add(sortOrder == "name" ? "name_desc" : "name", sortOrder == "name" ? "Names (A-Z)" : "Names (Z-A)");
            sortedParam.Add(sortOrder == "duration" ? "duration_desc" : "duration", sortOrder == "duration" ? "Less duratoion" : "Greater duration");
            sortedParam.Add(sortOrder == "amount" ? "amount_desc" : "amount", sortOrder == "amount" ? "Amount of students ascending" : "Amount of students descending");

            return new CourseListViewModel()
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                },
                CurrentParameter = sortingParam,
                SortedParam = sortedParam
            };

        }
        public IEnumerable<Course> SortingCourses(IEnumerable<Course> courses, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    courses = courses.OrderByDescending(s => s.CourseName).ToList();
                    break;
                case "name":
                    courses = courses.OrderBy(s => s.CourseName).ToList();
                    break;
                case "duration":
                    courses = courses.OrderBy(s => s.DurationDays).ToList();
                    break;
                case "duration_desc":
                    courses = courses.OrderByDescending(s => s.DurationDays).ToList();
                    break;
                case "amount":
                    courses = courses.OrderBy(s => s.AmountOfStudents).ToList();
                    break;
                case "amount_desc":
                    courses = courses.OrderByDescending(s => s.AmountOfStudents).ToList();
                    break;
                default:
                    courses = courses.OrderBy(s => s.Id).ToList();
                    break;
            }
            return courses.ToList();
        }
        public IEnumerable<Course> GetCourses( string sortParam)
        {
            var courses = UO.Courses.GetAll();
            if (!String.IsNullOrEmpty(sortParam))
            {
                List<Course> requiredCourses = new List<Course>();
                foreach (var course in courses)
                {
                    if (course.Teacher != null && course.Teacher.TeacherName == sortParam) requiredCourses.Add(course);
                }
                return requiredCourses;
            }
            return courses;
        }
  
        public Dictionary<int, bool> StudentRefisterPosibility(List<Course> courses)
        {
            string currentStudent = UO.NickName.GetAll()
                .Where(m => m.UserName == HttpContext.User.Identity.Name)
                .First().Learner.LearnerName;

            Dictionary<int, bool> studentRefisterPosibility = new Dictionary<int, bool>();

            foreach (var course in courses)
            {
                bool registerPosibility = UO.Logs.GetAll()
                    .Where(x => x.Lerner.LearnerName == currentStudent && x.Course.CourseName == course.CourseName)
                    .Count() != 0;
                studentRefisterPosibility.Add(course.Id, !registerPosibility);
            }

            return studentRefisterPosibility;
        }

    }

}

