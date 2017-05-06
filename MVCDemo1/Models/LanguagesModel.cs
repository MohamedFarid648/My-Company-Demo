using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDemo1.Models
{
    public class LanguagesModel
    {

        public List<System.Web.Mvc.SelectListItem> LanguagesINLanguagesClass { get; set; }
        public List<string> SelectedLanguages { get; set; }
    }
}
