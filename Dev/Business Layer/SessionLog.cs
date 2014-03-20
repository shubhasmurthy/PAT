using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Platform_Allocation_Tool.Data_Layer;

namespace Platform_Allocation_Tool.Business_Layer
{
    public class SessionLog
    {
        #region Attributes

		private String entryType;
		private String userName;
		private String serverName;
		private String platform;
		private String browser;
		private String stackTrace;
		private String message;
		private String assemblyVersion;
		private Int16 timeoutMinutes;
		private Int32 sessionLength;
		private String sessionID;
		private DateTime sessionStart;
		private String entryID;
		private DateTime entryDate;

		//private static string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();

		#endregion

		#region Properties

		public String EntryType
		{
			get { return entryType; }
			set { entryType = value; }
		}

		public String UserName
		{
			get { return userName; }
			set { userName = value; }
		}

		public String ServerName
		{
			get { return serverName; }
			set { serverName = value; }
		}

		public String Platform
		{
			get { return platform; }
			set { platform = value; }
		}

		public String Browser
		{
			get { return browser; }
			set { browser = value; }
		}

		public String StackTrace
		{
			get { return stackTrace; }
			set { stackTrace = value; }
		}

		public String Message
		{
			get { return message; }
			set { message = value; }
		}

		public String AssemblyVersion
		{
			get { return assemblyVersion; }
			set { assemblyVersion = value; }
		}

		public Int16 TimeoutMinutes
		{
			get { return timeoutMinutes; }
			set { timeoutMinutes = value; }
		}

		public Int32 SessionLength
		{
			get { return sessionLength; }
			set { sessionLength = value; }
		}

		public String SessionID
		{
			get { return sessionID; }
			set { sessionID = value; }
		}

		public DateTime SessionStart
		{
			get { return sessionStart; }
			set { sessionStart = value; }
		}

		public String EntryID
		{
			get { return entryID; }
			set { entryID = value; }
		}

		public DateTime EntryDate
		{
			get { return entryDate; }
			set { entryDate = value; }
		}

		#endregion

		#region Constructors

		public SessionLog(String uname)
		{		

			SessionStateSection sessionSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            this.entryID = uname;
			this.userName = User.GetName(this.entryID);
			this.serverName = System.Environment.MachineName;
			this.platform = HttpContext.Current.Request.Browser.Platform;
			this.browser = HttpContext.Current.Request.Browser.Browser;
			this.assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.timeoutMinutes = Convert.ToInt16(sessionSection.Timeout.TotalMinutes);
			this.sessionStart = Convert.ToDateTime(HttpContext.Current.Session["SessionStartTime"]);
			this.sessionLength = Convert.ToInt32(DateTime.Now.Subtract(this.sessionStart).TotalSeconds);
			this.sessionID = HttpContext.Current.Session.SessionID.ToString();
			this.entryDate = DateTime.Now;
		}

		#endregion


		#region ADO Methods

		public void Write(String type, String logMessage, String trace)
		{
            ConnectionData.WriteLog(this, type, logMessage, trace);
		}

		#endregion
    }
}