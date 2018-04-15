using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class EmployeeViewModel
    {
        //員編
        public string EmpId { get; set; }

        //員工姓名
        public string EmpNm { get; set; }

        //員工電話
        public string EmpPhone { get; set; }
    }
}