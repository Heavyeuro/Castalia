using Castalia.Domain.Entities;
using Castalia.Domain.Interfaces;
using Castalia.WEB.Infrastructure;
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
         [NotConfirmedNameException]
        public ViewResult SelectionByTopic(string sortingParam, string sortOrder, int page = 1)
        {

            ViewBag.sortingParam = sortingParam;
            //Selection of courses according sorting param
            var courses = UO.Courses.GetAll()
                   .Where(p => sortingParam == null || p.Topic.TopicName == sortingParam);

            //initializing view model
            CourseListViewModel model = CourseListInitializer(sortOrder, sortingParam, page);
            model.PagingInfo.TotalItems = courses.Count();

            //select courses that fit the paging condition
            model.Courses = SortingCourses(courses, sortOrder)
                   .Skip((page - 1) * PageSize)
                   .Take(PageSize).ToList();

            //opportunity for learner to register on course
            if (User.IsInRole("user"))
            {
                //in case if user don`t finish registration 
                //redirect him to view that gives that information 
            var learnerName = UO.NickName.GetAll()
                   .Where(m => m.UserName == HttpContext.User.Identity.Name)
                   .FirstOrDefault() ?? throw new NotConfirmedNameException(HttpContext.User.Identity.Name);
                model.StudentRefisterPosibility = StudentRefisterPosibility(model.Courses);
            }
            return View("TopicView", model);
        }

        /// <summary>
        /// Creates selection of all existed in database names of teachers
        /// </summary>
        [NotConfirmedNameException]
        public ViewResult SelectionByTeacher(string sortingParam, string sortOrder, int page = 1)
        {
            //return to the view sorting param to informate about current possition 
            ViewBag.sortingParam = sortingParam;

            //initializing view model
            CourseListViewModel model = CourseListInitializer(sortOrder, sortingParam, page);
            model.Courses = SortingCourses(GetCourses(sortingParam), sortOrder).ToList();
            model.PagingInfo.TotalItems = model.Courses.Count();

            //select courses that fit the paging condition
            model.Courses = model.Courses.Skip((page - 1) * PageSize)
                    .Take(PageSize).ToList();

            //opportunity for learner to register on course
            if (User.IsInRole("user"))
            {
                var learnerName = UO.NickName.GetAll()
                   .Where(m => m.UserName == HttpContext.User.Identity.Name)
                   .FirstOrDefault() ?? throw new NotConfirmedNameException(HttpContext.User.Identity.Name);
                model.StudentRefisterPosibility = StudentRefisterPosibility(model.Courses);
            }

            return View("TeacherView", model);
        }

        //redirect only for unexpected casess
        public ActionResult Index()
        {
            return RedirectToAction("SelectionByTopic");
        }

        public CourseListViewModel CourseListInitializer(string sortOrder, string sortingParam, int page = 1)
        {
            //initializing dictionary of sorting parameters
            Dictionary<string, string> sortedParam = new Dictionary<string, string>(3);
            sortedParam.Add(sortOrder == "name" ? "name_desc" : "name", sortOrder == "name" ? "Names (A-Z)" : "Names (Z-A)");
            sortedParam.Add(sortOrder == "duration" ? "duration_desc" : "duration", sortOrder == "duration" ? "Less duratoion" : "Greater duration");
            sortedParam.Add(sortOrder == "amount" ? "amount_desc" : "amount", sortOrder == "amount" ? "Amount of students ascending" : "Amount of students descending");
            
            //filling view model
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

        /// <summary>
        /// sorting courses according to given order
        /// </summary>
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

        /// <summary>
        /// geting list of courses besides courses without teacher
        /// </summary>
        public IEnumerable<Course> GetCourses(string sortParam)
        {
            var courses = UO.Courses.GetAll();
            if (!String.IsNullOrEmpty(sortParam))
            {//Filling list of courses besides null object of teacher
                List<Course> requiredCourses = new List<Course>();
                foreach (var course in courses)
                {
                    if ((course.Teacher != null) && (course.Teacher.TeacherName == sortParam)) requiredCourses.Add(course);
                }
                return requiredCourses;
            }
            return courses;
        }

        /// <summary>
        /// initializing dictionary that give information about current student according exact course
        /// </summary>
        public Dictionary<int, bool> StudentRefisterPosibility(List<Course> courses)
        {
            //geting current full name of the students
                string currentStudent = UO.NickName.GetAll()
                    .Where(m => m.UserName == HttpContext.User.Identity.Name)
                    .First().Learner.LearnerName;
                Dictionary<int, bool> studentRefisterPosibility = new Dictionary<int, bool>();
            //Fill the dictionary according with student possibility to register on course
                foreach (var course in courses)
                {
                    bool registerPosibility = UO.Logs.GetAll()
                        .Where(x => (x.Lerner.LearnerName == currentStudent) && (x.Course.CourseName == course.CourseName))
                        .Count() != 0;
                    studentRefisterPosibility.Add(course.Id, !registerPosibility);
                }
                return studentRefisterPosibility;
        }

    }

}

