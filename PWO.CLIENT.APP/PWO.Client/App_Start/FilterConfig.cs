using PWO.Client.App_Start;
using System.Web;
using System.Web.Mvc;

namespace PWO.Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new RefreshTokenAttribute());
        }
    }
}
