﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
     
        public ActionResult Table()
        {
            return View();
        }
        
        public ActionResult Forms()
        {
            return View();
        }     
        
        public ActionResult Panels()
        {
            return View();
        }

        public ActionResult Buttons()
        {
            return View();
        }

        public ActionResult Notifications()
        {
            return View();
        }

        public ActionResult Typography()
        {
            return View();
        }

        public ActionResult Icons()
        {
            return View();
        }

        public ActionResult Grid()
        {
            return View();
        }
    }
}