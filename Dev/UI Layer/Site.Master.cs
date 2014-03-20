using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            btnActive.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getOpenDemandRecords();
            }

        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            btnApproved.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
            IPageInterface pageInterface = Page as IPageInterface;
            if (pageInterface != null)
            {
                pageInterface.getApprovedDemandRecords();
            }

        }

        protected void btnNewDemand_Click(object sender, EventArgs e)
        {
            btnNewDemand.Attributes.Add("Style", "background-image: url('../Images/tabselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
        }
    }
}