using SyncAndAsyncHttpHandler.Base;
using SyncAndAsyncHttpHandler.HttpHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SyncAndAsyncHttpHandler
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Register();
        }

        private void Register()
        {
            // 1. Register Routes
            RouteTable.Routes.Add(CustomRouteGenerator.GetRoute<MyFirstHttpHandler>("MyFirstHttpHandler"));
            RouteTable.Routes.Add(CustomRouteGenerator.GetRoute<MyFirstAsyncHttpHandler>("MyFirstAsyncHttpHandler"));
            RouteTable.Routes.Add(CustomRouteGenerator.GetRoute<MyFirstAsyncHttpHandler>(""));

            // 2. Register Logging
            Logging.Setup(log4net.Core.Level.All);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}