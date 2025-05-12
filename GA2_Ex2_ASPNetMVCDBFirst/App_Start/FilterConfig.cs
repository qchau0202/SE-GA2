using System.Web;
using System.Web.Mvc;

namespace GA2_Ex2_ASPNetMVCDBFirst
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
