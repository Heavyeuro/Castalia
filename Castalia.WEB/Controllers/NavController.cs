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
        /// <param name="sortingParam"></param>
        /// <returns></returns>
        public PartialViewResult MenuTopic(string sortingParam = null)
        {
            ViewBag.SelectedTopic = sortingParam;

            IEnumerable<string> topics = repository.Courses.GetAll()
                .Select(p=>p.Topic.TopicName)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(topics);
        }

        /// <summary>
        /// Creates list of teacher ame links
        /// </summary>
        /// <param name="sortingParam"></param>
        /// <returns></returns>
        public PartialViewResult MenuTeacher(string sortingParam = null)
        {
            ViewBag.SelectedTeacher = sortingParam;

            IEnumerable<string> topics = repository.Teachers.GetAll().Select(p=>p.TeacherName).Distinct();
            return PartialView(topics);
        }
    }
}