using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;





namespace CRUDOperation.Controllers
{
    public class EmployeeController : Controller
       
    {
        private Employee1Entities db = new Employee1Entities();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllEmployee()
        {
            try
            {
                var employeeModels = GetAllEmployeeDetails();


                string text = RenderViewToString(this.ControllerContext, "_EmployeeDetails", employeeModels);
                return Json(new { Code = 0, Message = text});
            }
            
        
            catch(Exception ex)
            {
                return Json(new { Code = -1, Message = "Failed" });
            }
           // return employeeModels;
           
        }
        public JsonResult SaveEmployeeDetails(CRUDOperation.Models.EmployeeModel employeeModel)
        {
            Employee employee = new Employee()
            {
                FirstName = employeeModel.FirstName,
                LastName = employeeModel.LastName,
                Salary = employeeModel.Salary
            };
            db.Employees.Add(employee);
            this.db.SaveChangesAsync();

            return Json( new {code=0,Message="Saved" });

        }
        public JsonResult UpdateEmployee(CRUDOperation.Models.EmployeeModel employeeModel)
        {
            try
            {
                List<Employee> employeeList = db.Employees.Where(x => x.EmployeeId == employeeModel.EmployeeId).ToList();
                //  List<Employee> employeeList = IQuery.ToList();
                if (employeeList == null && employeeList.Count() == 0)
                {
                    throw new Exception("Employee not Exist");
                }
                else
                {
                    // employeeList.EmployeeId = employeeModel.EmployeeId;
                    employeeList[0].EmployeeId = employeeModel.EmployeeId;
                    employeeList[0].FirstName = employeeModel.FirstName;
                    employeeList[0].LastName = employeeModel.LastName;
                    employeeList[0].Salary = employeeModel.Salary;

                }

                this.db.SaveChanges();
                string text = RenderViewToString(this.ControllerContext, "_Update", employeeModel);
                return Json(new { Code = 0, Message = text });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = "Failed" });
            }


        }
        public JsonResult Delete(long id)
        {
            try
            {
                Employee employee = db.Employees.Find(id);
                //if (employee == null)
                //    throw new Exception("Equipment Not Exist");
                db.Employees.Remove(employee);
                db.SaveChanges();
                CRUDOperation.Models. EmployeeModel employeeModel = new Models.EmployeeModel();
                employeeModel.EmployeeList = GetAllEmployeeDetails();
                string text = RenderViewToString(this.ControllerContext, "_Delete", employeeModel);
                return Json(new { Code = 0, Message = text });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = "Failed" });
            }



        }
        public string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public JsonResult UpdatePopUp(CRUDOperation.Models.EmployeeModel employeeModel)
        {
            string convertedData = RenderViewToString(this.ControllerContext, "~/Views/Employee/_Update.cshtml", employeeModel);

            return Json(new { code = 0, message = convertedData });

        }
        public JsonResult DeletePopUp(long id)
        {
            CRUDOperation.Models.EmployeeModel employeeModel = new Models.EmployeeModel()
            {
                EmployeeId = id

            };
            string convertedData = RenderViewToString(this.ControllerContext, "~/Views/Employee/_Delete.cshtml", employeeModel);

            return Json(new { code = 0, message = convertedData });

        }



        public List<CRUDOperation.Models.EmployeeModel> GetAllEmployeeDetails()
        {
            IQueryable<Employee> IQuery = db.Employees;
            List<Employee> employees = IQuery.ToList();
            List<CRUDOperation.Models.EmployeeModel> employeeModels = new List<Models.EmployeeModel>();

            foreach (var item in employees)
            {
                CRUDOperation.Models.EmployeeModel employeeModel = new Models.EmployeeModel()
                {
                    EmployeeId = item.EmployeeId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Salary = item.Salary
                };
                employeeModels.Add(employeeModel);
            }
            return employeeModels;
        }

    }



}
