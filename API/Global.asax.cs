﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc; 
using System.Web.Routing;

namespace Emrys.API
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {  
            // API模式
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            // MVC模式
            //RouteConfig.RegisterRoutes(RouteTable.Routes); 
        }
    }
}