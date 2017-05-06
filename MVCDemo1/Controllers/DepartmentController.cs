using MVCDemo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo1.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index()
        {
            MyContextClass mcontextClass = new MyContextClass();
           List<Department> departments= mcontextClass.Departments.ToList();
            return View(departments);
        }
    }
}