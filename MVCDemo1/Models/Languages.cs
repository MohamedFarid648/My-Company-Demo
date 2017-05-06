using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCDemo1.Models
{
    public class Languages
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //        public bool isSelected { get; set; }
        public static string ReturnLanName(int lanID)
        {
            MyContextClass myCntextClass = new MyContextClass();
            Languages MyLanguages = myCntextClass.Languages.Single(de => de.ID == lanID);
            return (MyLanguages.Name);
        }
    }

}
