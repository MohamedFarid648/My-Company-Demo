using MVCDemo1.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
using System.Linq;
using System.Net;
namespace MVCDemo1.Controllers
{
    /*if we write any Action Filters (like:[ValidateAntiForgeryToken],[RequireHttps],...) Here,they
    /will be happened in all action methods*/
    [HandleError]//it will work(if mode="On" in customErrors tag in web.config) and go to Error.cshtml if any error happend exept 404,it will go to NotFound.cshtml
    public class EmployeeController : Controller
    {
        MyContextClass mcontextClass = new MyContextClass();


        /*ActionResult is the parent class you can write :
         * if(condition)
         *  return View();//ViewResult Object
         * else 
         *  return Json("Data") //JsonResult Object or RedirectResult,PartialViewResult
         *  you can use ILSpay tool
         */
        // GET: Employee
        public ActionResult Index(int depId)
        {
            //MyContextClass mcontextClass = new MyContextClass();
            List<Employee> employees = mcontextClass.Employees.Where(emp=>emp.DepartmentID==depId).ToList();
            return View(employees);
            /*
             
             A lambda expression is an anonymous function that you can use to create delegates or expression tree types. By using lambda expressions, you can write local functions that can be passed as arguments or returned as the value of function calls. Lambda expressions are particularly helpful for writing LINQ query expressions.
To create a lambda expression,
you specify input parameters (if any) on the left side of the lambda operator =>, 
and you put the expression or statement block on the other side. For example, 
the lambda expression x => x * x specifies a parameter that’s named x and returns the value of x squared.
You can assign this expression to a delegate type, as the following example shows:

            delegate int del(int i);  
            static void Main(string[] args)  
            {  
                del myDelegate = x => x * x;  
                int j = myDelegate(5); //j = 25  
            } 
            to more details:
            https://msdn.microsoft.com/en-us/library/bb397687.aspx
             */
        }


        // [RequireHttps]//we don't need it,htps is the default
        //  [OutputCache(Duration =30)]//will save data till 30 seconds
        public ActionResult AllEmployees(int? page)
        {
            //MyContextClass mcontextClass = new MyContextClass();

          IPagedList<Employee> employees = mcontextClass.Employees.ToList().ToPagedList(page ?? 1,4);
            //to make each 4 employees  in one page
            ViewBag.departmentTotal = Employee.getToatalEmplyeeInEachDep();

            return View(employees);
        }


       // [OutputCache(Duration = 30)]//will save data till 30 seconds
        public ActionResult AllEmployees2(string serachBy, string search, int? page)
        {

            //serachBy=Name or Gender
            //search=Ahmed,Sara,... Or Male,Female
            //MyContextClass mcontextClass = new MyContextClass();

            ViewBag.departmentTotal = Employee.getToatalEmplyeeInEachDep();
            if (serachBy == "Gender")
            {
                return View(mcontextClass.Employees.Where(x => x.Gender == search || search == null).ToList().ToPagedList(page ?? 1, 3));
                /*return all employees if SearchBy=Gender and Gender in DB=search or it can't found the Query(search=null)
                 search=Male return  males Employees,search=Female return  females Employees,search=null return all Employees 
             */
            }
            else
            {
                return View(mcontextClass.Employees.Where(x => x.Name.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 3));
                /*return all employees if SearchBy=Name and Name in  DB like search or it can't found the Query(search=null)
                 search=M return  all Names first letter in them (M) ,search=null return all Employees 
             */
            }

            //IPagedList<Employee> employees = mcontextClass.Employees.ToList().ToPagedList(page ?? 1, 4);
            //to make each 4 employees  in one page
        }


        [HttpGet]
        [ActionName("AllEmployees3")]
       // [OutputCache(Duration = 30)]//will save data till 30 seconds
        public ActionResult DeleteAllEmployees(string Name,string genders, int? page, int Dep=0)
        {
            //select departments
            //try { 
            ViewBag.Dep = new SelectList(mcontextClass.Departments, "ID", "Name");

            //select Genders
            var genderList = new List<string>();
            var genderQuery = from g in mcontextClass.Employees orderby g.Gender select g.Gender;
            //https://weblogs.asp.net/scottgu/dynamic-linq-part-1-using-the-linq-dynamic-query-library

            //it will select multiple male,female so:
            genderList.AddRange(genderQuery.Distinct());//AddRange to add all elements in a collection(list) to another one
            //now we just have one male,female
            ViewBag.genders = new SelectList(genderList);


            //select employees
            var employees = from e in mcontextClass.Employees select e;
            // IPagedList<Employee> employees = mcontextClass.Employees.ToList().ToPagedList(page ?? 1, 3);
            //I removed IPagedList because it sometimes doesn't work well with all searches
            if (!String.IsNullOrEmpty(Name)) {
                employees = employees.Where(x => x.Name.Contains(Name));
                //select * from Employee where Name like '"+Name+"'
                    //mcontextClass.Employees.Where(x => x.Name.Contains(Name)).OrderBy(i=>i.ID).ToPagedList(page ?? 1, 3);
            }

            if (!String.IsNullOrEmpty(genders))
            {
                employees = employees.Where(x => x.Gender == genders);
                    // mcontextClass.Employees.Where(x => x.Gender== genders).OrderBy(i => i.ID).ToPagedList(page ?? 1, 3);
            }

            if (Dep!=0)
            {
                employees = employees.Where(x => x.DepartmentID == Dep);
                    // mcontextClass.Employees.Where(x => x.DepartmentID == Dep).OrderBy(i => i.ID).ToPagedList(page ?? 1, 3);
            }
            return View(employees);
            /*}
            catch (Exception ex) {
                Response.Write("Error:<br/><font color='red'>" + ex.Message + "</font>");
                Response.Write("<script type='text/javascript' > alert('An error is happened please try again(Error:)"+ex.Message+" ');history.back();</script>") ;
                return View(mcontextClass.Employees.ToList());
            }
            */
        }
        [HttpPost]
        [ActionName("AllEmployees3")]
        public ActionResult DeleteAllEmployees(List<int> employeesIDs) {
          
            try
            {
                /* mcontextClass.Employees.Where(x => employeesIDs.Contains(x.ID)).ToList().ForEach(mcontextClass.Employees.DeleteObject);
                 mcontextClass.SaveChanges();
                 */
                /* string ids="";
                 foreach (int id in employeesIDs) {
                     // DeleteEmmployee(id);
                     ids += "Emp num:" + id+"<br/>";
                 }
                 return ids;*/

                if (employeesIDs.Count > 0)
                {
                    foreach (int id in employeesIDs)
                    {
                        Employee.DeleteEmmployee(id);
                    }
                }
                else {
                    Response.Write("<script type='text/javascript' > alert('An error is happened please try again ');history.back();</script>");

                }

           }
            catch (Exception ex) {
                Response.Write("Error:<font color='red'>" + ex.Message + "</font>");
                Response.Write("<script type='text/javascript' > alert('An error is happened please try again(Error:)" + ex.Message + " ');history.back();</script>");

            }
            return RedirectToAction("AllEmployees3");

        }



        public JsonResult GetEmployeesForAutoCompleteSearch(string term) {

            List<string> emps = mcontextClass.Employees.Where(x => x.Name.Contains(term)).Select(y => y.Name).ToList();
            return Json(emps, JsonRequestBehavior.AllowGet);
        }



        [NonAction]//That means that this action method can't invoked (or you can make the method private no public)
        public ActionResult TotalEmployeesInEachDepartment()
        {
            //MyContextClass mcontextClass = new MyContextClass();
            /*  List<DepartmentTotal> departmentTotal = mcontextClass.Employees.
                                                      GroupBy(x => x.DepartmentID).
                                                      Select(y => new DepartmentTotal { DepID = y.Key, Total = y.Count() }).
                                                      ToList().OrderByDescending(y=>y.Total);*/

           // ViewBag.departmentTotal = new List<string[]>();
            ViewBag.departmentTotal = Employee.getToatalEmplyeeInEachDep();

            return View();

        }


        public ActionResult Details(int id) {

           // MyContextClass mcontextClass = new MyContextClass();
            Employee employee = mcontextClass.Employees.Single(emp => emp.ID == id);
            return View(employee);
        }

        [HttpGet]//You don't need to write HttpGet because it is the default
        public ActionResult Create()
        {
            ViewBag.Dep = new SelectList(mcontextClass.Departments, "ID", "Name");
            ViewBag.ConfirmPassword = "";
            return View();
        }

        /*[HttpPost](Bad Code)
        public ActionResult Create(FormCollection formCollection)
        //formCollection will have Name,Email,Gender,... from your form when user click on create button
        // so you can write (string Name,..DateTime DateofBirth) instead of FormCollection formCollection
        
       {

        
       // foreach (string key in formCollection.AllKeys) {
         //   Response.Write("key:" + " ");
           // Response.Write(formCollection[key]);//formCollection[key] is the value(ex: Name is the key,Mohamed is the value)
            //Response.Write("<br/>");
        //}
        
          Employee emp = new Employee();
          emp.Name = formCollection["Name"];
          emp.Email = formCollection["Email"];
          emp.Address = formCollection["Address"];
          emp.Gender = formCollection["Gender"];
          emp.DateOfBirth = Convert.ToDateTime(formCollection["DateOfBirth"]);
          emp.DepartmentID = Convert.ToInt32(formCollection["DepartmentID"]);
          AddEmmployee(emp);

          return RedirectToAction("AllEmployees");
      }

      */

            [HttpPost]
        public ActionResult Create(Employee employee)
        {
           ViewBag.ConfirmPassword=Request.Form.Get("ConfirmPassword");
            //Response.Write("confirmPass is:" + ViewBag.ConfirmPassword);
            //[Bind(Include = "ID,Name,Email,Gender,DateOfBirth,Address,DepartmentID,Password")] Employee employee
                   if (mcontextClass.Employees.Any(x => x.Email == employee.Email)) {

                       ModelState.AddModelError("Email", "User Email is already exist");
                       Response.Write("Error:<br/><font color='red'>User Email is already exist</font>");


                   }
                   if (employee.Password != ViewBag.ConfirmPassword)
                   {
                       ModelState.AddModelError("ConfirmPassword", "Password doesn't match");
                       Response.Write("Error:<br/><font color='red'>Password doesn't match</font>");

                   }
                   if (ModelState.IsValid)
                   {
                       try
                       {
                           Employee.AddEmmployee(employee);
                           return RedirectToAction("AllEmployees");

                       }
                       catch (Exception ex)
                       {

                           Response.Write("Error:<br/><font color='red'>" + ex.Message + "</font>");
                           Response.Write("<br/><font color='blue'>Please Don't try to change anything in the System Please ^_^</font>");
                           return View();//return to the same page Create

                       }
                   }
                   else
                   {

                       return View();//return to the same page Create
                   }
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            // MyContextClass myContextClass = new MyContextClass();
            Employee employee = mcontextClass.Employees.Single(emp => emp.ID == id);
            ViewBag.ConfirmPassword = employee.Password;
            return View(employee);
        }



         [HttpPost]

         public ActionResult Edit(Employee employee)
         {
            ViewBag.ConfirmPassword = Request.Form.Get("ConfirmPassword");

            /*
             you can write url as:
             http://localhost:56341/Employee/Edit?id=3
             or
             http://localhost:56341/Employee/Edit/3
            
            // Response.Write(Request.Params.GetKey(1));will print ID(it is the first parameter in form)
            // ViewBag.ConfirmPassword=Request.Form["ConfirmPassword"];
            if (employee.Email != mcontextClass.Employees.Single(x => x.ID == employee.ID).Email) {
                ModelState.AddModelError("Email", "Don't change your email please ^_^");
                Response.Write("Error:<br/><font color='red'>Don't change your email please ^_^</font>");

            }*/

            if (employee.Password != ViewBag.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password doesn't match");
                Response.Write("Error:<br/><font color='red'>Password doesn't match</font>");

                Response.Write("<br/>Password :<font color='red'>" + employee.Password + "</font>");
                Response.Write("<br/>ConfirmPassword:<font color='red'>"+ ViewBag.ConfirmPassword + "</font>");

            }
            if (ModelState.IsValid)
             {
                 try
                 {
                    Employee.EditEmmployee(employee);
                     return RedirectToAction("AllEmployees");

                 }
                 catch (Exception ex)
                 {

                     Response.Write("Error:<br/><font color='red'>" + ex.Message+"</font>");
                     Response.Write("<br/><font color='blue'>Please Don't try to change anything in the System Please ^_^</font>");

                     return View(employee);

                 }
            }else
            {
                return View(employee);//return to the same page Edit/EmployeeID
            }
        }
        
  
        /* 
         *    [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var employeeToUpdate = mcontextClass.Employees.Find(id); 
            if (employeeToUpdate.Password != ViewBag.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password doesn't match");
                Response.Write("Error:<br/><font color='red'>Password doesn't match</font>");

            }
            if (TryUpdateModel(employeeToUpdate, "",
               new string[] { "Name", "Email", "Gender","DateOfBirth", "Address", "DepartmentID", "Password "}))
            {
                try
                {
                    /*As a best practice to prevent overposting, 
                     * the fields that you want to be updateable by the Edit page are whitelisted
                     *  in the TryUpdateModel*/
             /*       mcontextClass.SaveChanges();

                    return RedirectToAction("AllEmployees");
                }
                catch (DataException  dex )
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(employeeToUpdate);
        }*/

        /*  [HttpPost]
          [ActionName("Edit")]
          //but you must remove Required Data Annotation from Employee Class
          //excludin,including properties from model binding using bind attribute
          public ActionResult Edit_Post([Bind(Include= "ID,Email,Gender,DateOfBirth,Address,DepartmentID,Password,ConfirmPassword")] Employee employee)
          {
              employee.Name = mcontextClass.Employees.Single(empid => empid.ID == employee.ID ).Name;

              if (ModelState.IsValid)
              {
                  try
                  {
                      EditEmmployee(employee);
                      return RedirectToAction("AllEmployees");

                  }
                  catch (Exception ex)
                  {

                      Response.Write("Error:<br/><font color='red'>" + ex.Message + "</font>");
                      Response.Write("<br/><font color='blue'>Please Don't try to change anything in the System Please ^_^</font>");

                      return View(employee);

                  }
              }
              return View(employee);//return to the same page Edit/EmployeeID
          }

          */
        [HttpGet]
        public ActionResult Delete(int id)
        {
            // MyContextClass myContextClass = new MyContextClass();
            Employee employee = mcontextClass.Employees.Single(emp => emp.ID == id);
            return View(employee);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_Post(int id)
        {
            try {
                Employee.DeleteEmmployee(id);
                return RedirectToAction("AllEmployees");
            } catch (Exception ex) {
                Response.Write("Error:<font color='red'>" + ex.Message + "</font>");
                Response.Write("<script type='text/javascript' > alert('An error is happened please try again(Error:)" + ex.Message + " ');history.back();</script>");
                return RedirectToAction("AllEmployees");
            }  
                   

              
        }


        public JsonResult IsValidUserEmail(string Email) {

            return Json(!mcontextClass.Employees.Any(x => x.Email == Email),JsonRequestBehavior.AllowGet);

            /* why(!)?
             
             if email :mohamed@gmail.com is exist so mcontextClass.Employees.Any(x => x.Email == Email) 
             will return true 
             so when We asked IsValidUserEmail? it says yes(true) it is valid but it isn't. 
             */
        }

        public ActionResult AllEmployees4UsingAjax() {
            return View();
        }

        public PartialViewResult OrderByNamePartialView() {
            // IPagedList<Employee> MyEmployees = mcontextClass.Employees.ToList().ToPagedList(page?? 1,7);
            // var MyEmployees = from e in mcontextClass.Employees select  e;MyEmployees.ToList()

            //it doesn't work well
            // List<Employee> MyEmployees = mcontextClass.Employees.ToList();
            
            //it works well
            List<Employee> MyEmployees = mcontextClass.Employees.OrderBy(x => x.Name).Take(7).ToList();

            return PartialView("UsingAjax", MyEmployees);
        }

        public PartialViewResult OrderByIDPartialView( )
        {
            //IPagedList<Employee> Emplloyees = mcontextClass.Employees.OrderBy(x => x.ID).Take(5).ToList().ToPagedList(page ?? 1, 6);

            List<Employee> MyEmployees = mcontextClass.Employees.OrderBy(x => x.ID).Take(7).ToList();

            return PartialView("UsingAjax", MyEmployees);
        }

        public PartialViewResult OrderByAgePartialView()
        {
            //IPagedList<Employee> Emplloyees = mcontextClass.Employees.OrderByDescending(x => x.ID).Take(5).ToList().ToPagedList(page ?? 1, 6);
            // var Emplloyees = from e in mcontextClass.Employees  orderby mcontextClass.Employees.Single().ID descending select Top 5 e;

            //it doesn't work well
            //List<Employee> MyEmployees = mcontextClass.Employees.OrderByDescending(x => x.ID).Take(5).ToList();//wanted to get last 5 employees
            
            //it works well
            List<Employee> MyEmployees = mcontextClass.Employees.OrderBy(x => x.DateOfBirth).Take(7).ToList();

            return PartialView("UsingAjax", MyEmployees);
        }


        public ActionResult Login() {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] Employee employee)
        {
            Employee empLogin = null;
            try {
                 empLogin = mcontextClass.Employees.Single(m => m.Email == employee.Email && m.Password == employee.Password);

            }
            catch (Exception ex)
            {
               // Response.Write("Error:<br/><font color='red'>" + ex.Message + "</font>");
                Response.Write("<br/><font color='red'> Some of your Information are incorrect,try again  Please ^_^</font>");
                //Response.Write("<br/>Error:<font color='red'>"+ex.Message+"</font>");
                return View();//return to the same page Create
            }
           /* if (String.IsNullOrEmpty(empLogin.Password.ToString())) {
                ModelState.AddModelError("Password", "Password is incorrect");
                Response.Write("Error:<br/><font color='red'>Password is incorrect</font>");

            }
            if (String.IsNullOrEmpty(empLogin.Email.ToString()))
            {
                ModelState.AddModelError("Email", "Email is incorrect");
                Response.Write("Error:<br/><font color='red'>Email is incorrect</font>");

            }*/
            //Response.Write("Email is:" + empLogin.Email);
            //Response.Write("<br/>Pass is:" + empLogin.Password);
            //if (ModelState.IsValid)
            //{
                try
                {
                    Session["UserSessionEmail"] = empLogin.Email;
                    Session["UserSessionName"] = empLogin.Name;
                    Session["UserSessionID"] = empLogin.ID;
                    return RedirectToAction("AllEmployees");

                }
                catch (Exception ex)
                {

                    Response.Write("Error:<br/><font color='red'>" + ex.Message + "</font>");
                    Response.Write("<br/><font color='blue'>Please Don't try to change anything in the System Please ^_^</font>");
                    return View();//return to the same page Create

                }
         /*   }
            else
            {
                Response.Write("Error:<br/><font color='red'>Model is invalid,there are some errors</font>");
                return View();
            }*/


        }

        public ActionResult Register()
        {
            return View();

        }
   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID,Name,Email,Gender,DateOfBirth,Address,DepartmentID,Password")] Employee employee)
        {

            ViewBag.ConfirmPassword = Request.Form.Get("ConfirmPassword");
            //Response.Write("confirmPass is:" + ViewBag.ConfirmPassword);
            //[Bind(Include = "ID,Name,Email,Gender,DateOfBirth,Address,DepartmentID,Password")] Employee employee
            if (mcontextClass.Employees.Any(x => x.Email == employee.Email))
            {

                ModelState.AddModelError("Email", "User Email is already exist");
                Response.Write("Error:<br/><font color='red'>User Email is already exist</font>");


            }
            if (employee.Password != ViewBag.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password doesn't match");
                Response.Write("Error:<br/><font color='red'>Password doesn't match</font>");

                Response.Write("<br/>Password :<font color='red'>" + employee.Password + "</font>");
                Response.Write("<br/>ConfirmPassword:<font color='red'>" + ViewBag.ConfirmPassword + "</font>");

            }
            if (ModelState.IsValid)
            {
                try
                {
                    Session["UserSessionEmail"] = employee.Email;
                    Session["UserSessionName"] = employee.Name;
                    Session["UserSessionID"] = employee.ID;
                    mcontextClass.Employees.Add(employee);
                    mcontextClass.SaveChanges();
                    return RedirectToAction("AllEmployees");

                }
                catch (Exception ex)
                {

                    Response.Write("Error:<br/><font color='red'>" + ex.Message + "</font>");
                    Response.Write("<br/><font color='blue'>Please Don't try to change anything in the System Please ^_^</font>");
                    return View();//return to the same page Create

                }
            }
            else
            {

                return View();//return to the same page Create
            }

        }

        public ActionResult Logout()
        {
            try
            {
                Session.Abandon();
            }
            catch (Exception ex) {

            }
            return RedirectToAction("AllEmployees");

        }
    }
}









