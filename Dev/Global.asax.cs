﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Platform_Allocation_Tool;
using Platform_Allocation_Tool.Business_Layer;

namespace Platform_Allocation_Tool
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			// Code that runs on application startup
			//BundleConfig.RegisterBundles(BundleTable.Bundles);
			//AuthConfig.RegisterOpenAuth();
			//RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		void Application_End(object sender, EventArgs e)
		{
			//  Code that runs on application shutdown

		}

		void Application_Error(object sender, EventArgs e)
		{
			// Code that runs when an unhandled error occurs
            

		}

        void Session_Start(object sender, EventArgs e)
        {
            Session["SessionStartTime"] = DateTime.Now;

     


           // SessionLog log = new SessionLog();
            //Session["log"] = log;

            //			SessionManager.Instance["SessionStartTime"] = DateTime.Now;
           // log.Write("Info", "New session", String.Empty);
        }
        void Session_End(object sender, EventArgs e)
        {

            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.


            			//HttpContext.Current.Response.Redirect("UI/SessionTimedout.aspx");
        }
	}
}
