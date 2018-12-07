using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDOperation.Models
{
    public class EmployeeModel
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public List<EmployeeModel> EmployeeList { get; set; }
    }
}