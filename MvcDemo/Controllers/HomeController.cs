using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDemo.Repository;
using MvcDemo.Models;

namespace MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        private EmployeeRepository Repository = new EmployeeRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult EmpQueryData(EmployeeViewModel viewModel)
        {
            try
            {
                List<EmployeeModels> empList = Repository.EmpQueryByAdoNet(viewModel);
                return Json(empList);
            }
            catch (Exception ex)
            {
                //Write Log..
                throw ex;
            }
            
        }
    }
}