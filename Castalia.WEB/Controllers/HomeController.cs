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
        /// <param name="sortingParam"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ViewResult SelectionByTopic(string sortingParam, string sortOrder, int page = 1)
        {
            int amountOfItems = 0;
            if (String.IsNullOrEmpty(sortingParam)) amountOfItems = amountOfItems = UO.Courses.GetAll().Count();
            else
                foreach (var a in UO.Topics.GetAll())
                    if (a.TopicName == sortingParam) amountOfItems++;
            CourseListViewModel model = CourseListInitializer(amountOfItems, sortOrder, sortingParam, page);

            model.Courses = SortingCourses(UO.Courses.GetAll()
                   .Where(p => sortingParam == null || p.Topic.TopicName == sortingParam)
                   , sortOrder).Skip((page - 1) * PageSize)
                   .Take(PageSize).ToList();

            if (User.IsInRole("student")) model.StudentRefisterPosibility = StudentRefisterPosibility(model.Courses);

            return View("TopicView", model);
        }

        /// <summary>
        /// Creates selection of all existed in database names of teachers
        /// </summary>
        /// <param name="sortingParam"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ViewResult SelectionByTeacher(string sortingParam, string sortOrder, int page = 1)
        {
            int amountOfItems = 0;
            if (String.IsNullOrEmpty(sortingParam)) amountOfItems = UO.Courses.GetAll().Count();
            else
                foreach (var a in UO.Teachers.GetAll())
                    if (a.TeacherName == sortingParam) amountOfItems++;


            CourseListViewModel model = CourseListInitializer(amountOfItems, sortOrder, sortingParam, page);

            model.Courses = SortingCourses(GetCourses(UO.Courses.GetAll(), sortingParam)
                    , sortOrder).Skip((page - 1) * PageSize)
                    .Take(PageSize).ToList();

            //if(User.IsInRole("student"))
            model.StudentRefisterPosibility = StudentRefisterPosibility(model.Courses);

            return View("TeacherView", model);
        }


        public ActionResult Index()
        {
            return RedirectToAction("SelectionByTopic");
        }

        public CourseListViewModel CourseListInitializer(int amountOfItems, string sortOrder, string sortingParam, int page = 1)
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
                    TotalItems = amountOfItems
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
        public IEnumerable<Course> GetCourses(IEnumerable<Course> courses, string sortParam)
        {
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

        //implement it in view (if null dont display) 1- "can registre", 0- "already registred"
        public Dictionary<int, bool> StudentRefisterPosibility(List<Course> courses)
        {
            string currentStudent = HttpContext.User.Identity.Name;
            //temp
            currentStudent = "Ivanov Ivan";
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

