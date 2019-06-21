using Castalia.Domain.Entities;
using Castalia.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Castalia.WEB.Controllers
{
    public class SelfCabinetController : Controller
    {
        IUnitOfWork UO;

        public SelfCabinetController(IUnitOfWork repo)
        {
            UO = repo;
        }

        [HttpGet]
        public ActionResult Index()
        {
            NicknameName currentUser = UO.NickName.GetAll()
            .Where(m => m.UserName == HttpContext.User.Identity.Name)
            .FirstOrDefault();

            //if (User.IsInRole("user"))
            //{
            //    //if (currentUser == null)
            //    //    currentUser = "Please,enter your full name";

            //}
            return View(currentUser);//(currentUser);
          
        }

        //[HttpPost]
        //public ActionResult Index()
        //{
        //    var currentUser = UO.NickName.GetAll()
        //    .Where(m => m.UserName == HttpContext.User.Identity.Name)
        //    .FirstOrDefault().Learner;

        //    if (User.IsInRole("user"))
        //    {
        //        if (currentUser == null)

        //    }
        //    return View();
        //    //    if (User.IsInRole("teacher"))


        //    return View();
        //}

        [HttpPost]
        public ActionResult AddFullName(string FullName,NicknameName currentUser)
        {
            //validation
            try
            {
                if (UO.NickName.GetAll().Where(x => x.Learner.LearnerName == FullName).First() != null)
                    ModelState.AddModelError("", "This name is already used");
            }
            catch (NullReferenceException) { }
         
            if (String.IsNullOrEmpty(FullName))
                ModelState.AddModelError("", "Please enter your full name");

            if (FullName.All(x => !(char.IsLetter(x) || char.IsWhiteSpace(x) || x == '-')))
                ModelState.AddModelError("", "Unacceptable symbols detected");

            if (ModelState.IsValid)
            { 
                 var learner = new Learner()
                {
                    IsBlocked = false,
                    LearnerName = FullName
                };

                currentUser = new NicknameName() {
                    UserName = HttpContext.User.Identity.Name,
                    Learner=learner
                };
                UO.NickName.Create(currentUser);
                UO.Save();
                return View("Index", currentUser);
            }
                return View("Index");
        }

    }
}