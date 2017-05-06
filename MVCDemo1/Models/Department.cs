using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDemo1.Models
{
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Employee> DepartmentEmplyees { get; set; }

        public static string ReturnDepName(int DepID)
        {
            MyContextClass myCntextClass = new MyContextClass();
            Department MyDepartment = myCntextClass.Departments.Single(de => de.ID == DepID);
            return (MyDepartment.Name);
        }
    }
}
