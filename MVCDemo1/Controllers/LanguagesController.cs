using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDemo1.Models;
using System.Text;

namespace MVCDemo1.Controllers
{
    public class LanguagesController : Controller
    {
        // GET: Languages
        [HttpGet]
        public ActionResult Index()
        {
            MyContextClass mcontextClass = new MyContextClass();
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (Languages l in mcontextClass.Languages) {
                //create: C# 1 true(selected),C 2 false(notselected),Java 3 true ,...
                SelectListItem selectListItem = new SelectListItem() {
                    Text=l.Name,
                    Value=l.ID.ToString(),
                   // Selected=l.isSelected
                };
                //put each row(Java 3 true) in the list
                selectListItems.Add(selectListItem);
            }//end of foreach

            LanguagesModel languagesModel = new LanguagesModel();
            languagesModel.LanguagesINLanguagesClass = selectListItems;
            //send languagesModel (that have all languages in LanguagesINLanguagesClass object,and will has in view all selected languages in SelectedLanguages Object

            return View(languagesModel);
        }



        [HttpPost]
        public string Index(List<string> SelectedLanguages)
        {
            if (SelectedLanguages == null) { return "Choose  language ,please ^_^"; }
            else {
                StringBuilder sb = new StringBuilder();
                sb.Append("You selected(Names):");
                foreach (string s in SelectedLanguages) {
                   // Response.Write(s + "<br/>");
                    sb.Append(Languages.ReturnLanName(Convert.ToInt32(s))+",");
                }
                sb.Remove(sb.ToString().LastIndexOf(","),1);
                //to remove last (,)
                //1 is the num of character that we want to remove

                sb.Append("<br/>");
             sb.Append("You selected(IDs):" + string.Join(",",SelectedLanguages));

                return sb.ToString();
            }

            }
        }
   
}