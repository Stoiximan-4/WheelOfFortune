﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminMonitorApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }
        
        public ActionResult WheelSpinsConfiguration()
        {
            return View();
        }

        public ActionResult CouponsConfiguration()
        {
            return View();
        }
    }
}