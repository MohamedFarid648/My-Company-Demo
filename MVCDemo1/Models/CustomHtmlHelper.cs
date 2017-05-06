using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo1.Models
{
    public static class CustomHtmlHelper
    {
        public static IHtmlString Image(this HtmlHelper helper,string src,string alt,int height,int width) {

            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("alt", alt);
            tb.Attributes.Add("height", height.ToString());
            tb.Attributes.Add("width", width.ToString());

            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }
    }
}
