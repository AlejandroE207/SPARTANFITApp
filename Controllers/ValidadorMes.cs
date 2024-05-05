using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPARTANFITApp.Controllers
{
    


    public class BaseController : Controller
        {
            protected void SetNoCacheHeaders()
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1)); 
                Response.Cache.SetNoStore();
            }

            protected override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                SetNoCacheHeaders();

                base.OnActionExecuting(filterContext);
            }
        }
}
