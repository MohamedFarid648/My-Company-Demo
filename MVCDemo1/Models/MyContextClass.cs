using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MVCDemo1.Models
{
    public class MyContextClass:DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Languages> Languages { get; set; }

        /*
          To Solve error:
          * The model backing the 'Context Class' context has changed since the database was created. 
          * Consider using Code First Migrations to update the database

          */
        // Add this to Application_Start () in Glopal.asax: 
        //Database.SetInitializer<MVCDemo1.Models.MyContextClass>(null);
        /*End*/

        /*To Solve error:
         * The model backing the 'Context Class' context has changed since the database was created. 
         * Consider using Code First Migrations to update the database
         
         
         protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
    Database.SetInitializer<YourDbContext>(null);
    base.OnModelCreating(modelBuilder);
}
         */
    }
}
