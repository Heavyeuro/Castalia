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

        public ActionResult Index()
        {
           
            //if (User.IsInRole("user"))
            //    return View();
            //    if (User.IsInRole("teacher"))


            return View();
        }

    }
}