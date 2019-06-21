using Castalia.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Castalia.WEB.Controllers
{

    public class NavController : Controller
    {
        private IUnitOfWork repository;

        public NavController(IUnitOfWork repo)
        {
            repository = repo;
        }

        /// <summary>
        /// Creates list of topic name links
        /// </summary>
        public PartialViewResult MenuTopic(string sortingParam = null)
        {
            ViewBag.SelectedTopic = sortingParam;
            //geting all topics that bounded with courses
            IEnumerable<string> topics = repository.Courses.GetAll()
                .Select(p => p.Topic.TopicName)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(topics);
        }

        /// <summary>
        /// Creates list of teacher name links
        /// </summary>
        public PartialViewResult MenuTeacher(string sortingParam = null)
        {
            ViewBag.SelectedTeacher = sortingParam;
            //geting all names of the teachers that bounded with courses
            IEnumerable<string> teacherName = repository.Teachers.GetAll().Select(p => p.TeacherName).Distinct();
            return PartialView(teacherName);
        }
    }
}