﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WheelOfFortune.Controllers
{
    public class TransactionsController : Controller
    {
        
        public ActionResult ShowTransactionsHistory()
        {
            return View();
        }
    }
}