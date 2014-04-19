using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Platform_Allocation_Tool
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnActive_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnActive.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getOpenDemandRecords();
            }

        }


        protected void renderButtons(){
            btnNewDemand.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnWaitingApproval.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnActive.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnSaved.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnDeclined.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnToBeClaimed.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnToBeApproved.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnApproved.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnClosed.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnOrdered.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            btnMaintenance.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
        }
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnApproved.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getApprovedDemandRecords();
            }

        }

        protected void btnWaitingApproval_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnWaitingApproval.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getSavedDemandRecords();
            }

        }

        protected void btnNewDemand_Click(object sender, EventArgs e)
        {
           // renderButtons();
           // btnNewDemand.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.showNewDemandWindow();
            }
        }

        protected void btnSaved_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnSaved.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getSavedDemandRecords();
            }
        }

        protected void btnDeclined_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnDeclined.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getDeclinedDemandRecords();
            }
        }

        protected void btnToBeClaimed_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnToBeClaimed.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getOpenDemandRecords();
            }

        }

        protected void btnToBeApproved_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnToBeApproved.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getSavedDemandRecords();
            }
        }

        protected void btnClosed_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnClosed.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getClosedDemandRecords();
            }
        }

        protected void btnOrdered_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnOrdered.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getOrderedDemandRecords();
            }

        }

        protected void btnMaintenance_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnMaintenance.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.AdminUtility();
            }

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            renderButtons();
            btnLogout.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.LogOut();
            }
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");
        }
    }
}