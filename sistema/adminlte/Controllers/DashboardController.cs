﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminlte.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboardv1()
        {
            if (Session["usuarioLogado"] != null)
                return View();
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult Dashboardv2()
        {
            return View();
        }
    }
}