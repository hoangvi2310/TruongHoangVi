using System.Web;
using System.Web.Mvc;

namespace _15DH110184_HoangVi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
