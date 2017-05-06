using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;
using MVCDemo1.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCDemo1.Models
{
    /* [MetadataType(typeof(EmployeeMetaData))]
     public partial class Employee {

         public string ConfirmEmail { get; set; }
     }*/
    public class Employee
    {
        public int ID { get; set; }


        [StringLength(50, MinimumLength = 7)]//Name must be between 7 and 50 characters
        [Required(ErrorMessage = "Enter Employee Name Please ^_^")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Enter a valid Name please ^_^")]
        /*
         RegularExpression:for more details:

            https://msdn.microsoft.com/en-us/library/az24scfc.aspx

            or from Community tab here you can search about email,name,...:
            http://regexr.com/v1/

             */
        //@ because the compiler doesn't know the meaning of  \s

        public string Name { get; set; }


        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Enter a valid Email please ^_^")]
        /*
         * (.)means one digit(num,letters,...) ex:m
           (+)means more than one ex:mohamed
           (\\@) means @ ex:mohamed@ 
           (.+) the same thing (more than one digits) ex:mohamed@gmail
           (\\.) means . ex:mohamed@gmail.
           (.+) the same thing (more than one digits) ex:ex:mohamed@gmail.com.edu.eg...
         */
        [Required(ErrorMessage = "Enter Employee Email Please ^_^")]
        [DataType(DataType.EmailAddress)]
        // [ReadOnly(true)]//using System.ComponentModel; if you write it he can't edit it 
        /*[Remote("IsValidUserEmail","Employee",ErrorMessage = "User Email is already exist")]
        it doesn't work if javascript is disapled
        (IsValidUserEmail) is the name of the method in (Employee) Controller
        
             It works well in Create but in Edit it does a logical error(user can't change his data because his email in the system)
             */
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 7)]//Password must be between 7 and 50 characters
        [Required(ErrorMessage = "Enter Employee Password Please ^_^")]
        public string Password { get; set; }
        /*
                [DataType(DataType.Password)]
                [StringLength(50, MinimumLength = 7)]//Password must be between 7 and 50 characters
                [Required(ErrorMessage = "Confirm Your Password Please ^_^")]
                public string ConfirmPassword { get; set; }
                */

        [Required(ErrorMessage = "Enter Employee Gender Please ^_^")]
        public string Gender { get; set; }

        [Display(Name = "Born in:")]
        [Required(ErrorMessage = "Enter Employee Birth Date Please ^_^")]
        [DataType(DataType.Date)]//works in Dispaly and makes calender in Create but bad view in Edit
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//to appear just date //it doesn't work
                                                                                          // [Range(typeof(DateTime),"01/01/1957","12/30/1999")] 
                                                                                          /*Month/day/year
                                                                                          it doesn't work well so we create our custom validation attribute:*/
        [CustomRangeDateValidation("01/01/1954")]//it  works well,it accepts any date between 01/01/1954 and  CurrentMonth/CurrentDay/CurrentYear
        //[CurrentDate]//it works well it  accepts any date  untill CurrentMonth/CurrentDay/CurrentYear
        public DateTime DateOfBirth { get; set; }

        [ScaffoldColumn(false)]//doesn't work
        [Required(ErrorMessage = "Enter Employee Address Please ^_^")]
        //[DisplayColumn("")]
        // [HiddenInput(DisplayValue = false)]//using System.Web.Mvc
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter Employee Department Please ^_^")]
        [Display(Name = "Department Name")]//will appear to user Department Name not DepartmentID

        public int DepartmentID { get; set; }

        public string Photo { get; set; }
        public string PhotoText { get; set; }

        // methods 
        public static void AddEmmployee(Employee employee)
        {
            string connectionString =
                    ConfigurationManager.ConnectionStrings["MyContextClass"].ConnectionString;//MyContextClass the name in AddString in Web.config

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ProcedureCreateEmployee", con);//[ProcedureCreateEmployee] is Stored Procedure Name
                cmd.CommandType = CommandType.StoredProcedure;
                /*@Name ,
                  @Email,
                  @Address,
                  @Gender,
                  @DateOfBirth ,
                  @DepartmentID ,@Password*/
                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Name";
                paramName.Value = employee.Name;
                cmd.Parameters.Add(paramName);

                SqlParameter paramEmail = new SqlParameter();
                paramEmail.ParameterName = "@Email";
                paramEmail.Value = employee.Email;
                cmd.Parameters.Add(paramEmail);


                SqlParameter paramGender = new SqlParameter();
                paramGender.ParameterName = "@Gender";
                paramGender.Value = employee.Gender;
                cmd.Parameters.Add(paramGender);



                SqlParameter paramDateOfBirth = new SqlParameter();
                paramDateOfBirth.ParameterName = "@DateOfBirth";
                paramDateOfBirth.Value = employee.DateOfBirth;
                cmd.Parameters.Add(paramDateOfBirth);


                SqlParameter paramAddress = new SqlParameter();
                paramAddress.ParameterName = "@Address";
                paramAddress.Value = employee.Address;
                cmd.Parameters.Add(paramAddress);



                SqlParameter paramDepartmentID = new SqlParameter();
                paramDepartmentID.ParameterName = "@DepartmentID";
                paramDepartmentID.Value = employee.DepartmentID;
                cmd.Parameters.Add(paramDepartmentID);

                SqlParameter paramPassword = new SqlParameter();
                paramPassword.ParameterName = "@Password";
                paramPassword.Value = employee.Password;
                cmd.Parameters.Add(paramPassword);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static void EditEmmployee(Employee employee)
        {
            string connectionString =
                    ConfigurationManager.ConnectionStrings["MyContextClass"].ConnectionString;//MyContextClass the name in AddString in Web.config

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ProcedureEditEmployee", con);//[ProcedureEditEmployee] is Stored Procedure Name
                cmd.CommandType = CommandType.StoredProcedure;
                /*	@ID int,
	                @Name nvarchar(Max),
	                @Email nvarchar(Max),
	                @Gender nvarchar(Max),
	                @DateOfBirth datetime,
	                @Address nvarchar(Max),
	                @DepartmentID int
                    @Password   nvarchar(Max)
                 */
                SqlParameter paramID = new SqlParameter();
                paramID.ParameterName = "@ID";
                paramID.Value = employee.ID;
                cmd.Parameters.Add(paramID);

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Name";
                paramName.Value = employee.Name;
                cmd.Parameters.Add(paramName);

                SqlParameter paramEmail = new SqlParameter();
                paramEmail.ParameterName = "@Email";
                paramEmail.Value = employee.Email;
                cmd.Parameters.Add(paramEmail);


                SqlParameter paramGender = new SqlParameter();
                paramGender.ParameterName = "@Gender";
                paramGender.Value = employee.Gender;
                cmd.Parameters.Add(paramGender);



                SqlParameter paramDateOfBirth = new SqlParameter();
                paramDateOfBirth.ParameterName = "@DateOfBirth";
                paramDateOfBirth.Value = employee.DateOfBirth;
                cmd.Parameters.Add(paramDateOfBirth);


                SqlParameter paramAddress = new SqlParameter();
                paramAddress.ParameterName = "@Address";
                paramAddress.Value = employee.Address;
                cmd.Parameters.Add(paramAddress);



                SqlParameter paramDepartmentID = new SqlParameter();
                paramDepartmentID.ParameterName = "@DepartmentID";
                paramDepartmentID.Value = employee.DepartmentID;
                cmd.Parameters.Add(paramDepartmentID);

                SqlParameter paramPassword = new SqlParameter();
                paramPassword.ParameterName = "@Password";
                paramPassword.Value = employee.Password;
                cmd.Parameters.Add(paramPassword);


                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static void DeleteEmmployee(int id)
        {
            string connectionString =
                    ConfigurationManager.ConnectionStrings["MyContextClass"].ConnectionString;//MyContextClass the name in AddString in Web.config

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ProcedureDeleteEmployee", con);//[ProcedureDeleteEmployee] is Stored Procedure Name
                cmd.CommandType = CommandType.StoredProcedure;
                /*	@ID int,
	              */
                SqlParameter paramID = new SqlParameter();
                paramID.ParameterName = "@ID";
                paramID.Value = id;
                cmd.Parameters.Add(paramID);


                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static List<string[]> getToatalEmplyeeInEachDep()
        {
            string connectionString =
                   ConfigurationManager.ConnectionStrings["MyContextClass"].ConnectionString;//MyContextClass the name in AddString in Web.config

            List<string[]> depTotals = new List<string[]>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ProcedureGetEmployeeDepartmentTotal", con);//[ProcedureGetEmployeeDepartmentTotal] is Stored Procedure Name
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string DepName = rdr["Name"].ToString();
                    string Total = rdr["Total"].ToString();
                    string[] row = { DepName, Total };
                    depTotals.Add(row);
                }
            }
            return depTotals;

        }

    }

}
