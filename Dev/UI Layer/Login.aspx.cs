using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Platform_Allocation_Tool;
using Platform_Allocation_Tool.Business_Layer;

namespace Platform_Allocation_Tool.UI_Layer
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Button myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnNewDemand");
            myButton.Visible = false;
            myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnActive");
            myButton.Visible = false;
            myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnApproved");
            myButton.Visible = false;
            myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeClaimed");
            myButton.Visible = false;
            myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnOrdered");
            myButton.Visible = false;
            myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeApproved");
            myButton.Visible = false;
            myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnClosed");
            myButton.Visible = false;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            String uname = "";
            String pwd = "";
            uname = txtName.Text.Trim();
            pwd = txtPwd.Text.Trim();
            try
            {
                User user = new User(uname, pwd);
                Session["user"] = user;

                SessionLog log = new SessionLog(uname);
                Session["log"] = log;

                log.Write("Info", "New Session", String.Empty);

                FormsAuthentication.RedirectFromLoginPage(uname, true);
            }
            catch (UnauthorizedAccessException ex)
            {
                Response.Redirect("UnauthorisedAccessErrorPage.aspx");
            }
            catch
            {
                Response.Redirect("GenericErrorPage.aspx");
            }
        }

    }
}