using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDemo.Repository;
using MvcDemo.Models;
using Newtonsoft.Json;

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

        public PartialViewResult ShowEmpPartial(string View, string RowData)
        {
            try
            {
                //ADD MODE
                if (RowData == null)
                {
                    return PartialView(View);
                }


                EmployeeModels employeeModel = JsonConvert.DeserializeObject<EmployeeModels>(RowData);
                return PartialView(View, employeeModel);
            }
            catch (Exception ex)
            {
                //write log
                throw ex;
            }
        }

        public ActionResult SaveData(EmployeeModels Model)
        {
            try
            {
                //沒有ID,代表新增
                if (Model.EmployeeID == null) { Repository.InsertEmpData(Model); }
                else { Repository.updateEmpData(Model); }
                
                return Content("1");
            }
            catch (Exception ex)
            {
                //WriteLog..
                return Content("0");
            }
        }
    }
}