using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Net.Mail;
using Platform_Allocation_Tool.Business_Layer;
using Platform_Allocation_Tool.Data_Layer;
using System.Threading;

namespace Platform_Allocation_Tool
{

    public partial class Default : System.Web.UI.Page, IPageInterface
    {
        protected User user;
        protected SessionLog log;
        //  protected List<TeamBoard> tbList = new List<TeamBoard>();
        protected List<TeamBoard> tbListDummy = new List<TeamBoard>();
        protected List<TechnicalDocumentation> tdListDummy = new List<TechnicalDocumentation>();
        static Demand demand;
        static String status="";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] == null)
                Response.Redirect("Login.aspx");

            try
            {
                user = (User)Session["user"];
                log = (SessionLog)Session["log"];
                if (status.Equals("")) 
                {
                    if (user.Role.Equals("Administrator") || user.Role.Equals("TeamRep"))
                    {
                        status = "Open";
                        getOpenDemandRecords();
                    }
                    else
                    {
                        status = "Saved";
                        getSavedDemandRecords();
                    }
                }
                

                log.Write("Info", "Page View -> Home", String.Empty);

                if (!IsPostBack)
                {
                    List<TeamBoard> tbList = new List<TeamBoard>();
                    ViewState["BoardList"] = tbList;
                    List<TechnicalDocumentation> tdList = new List<TechnicalDocumentation>();
                    ViewState["TechDoc"] = tdList;

                    if (ViewState["Demand"] != null)
                    {
                        demand = ViewState["Demand"] as Demand;
                    }

                    Demand.MonitorClosedDemands();
                    //refreshGrid();
                    renderView();
                    
                }

            }
            catch (NullReferenceException ex)
            {
                //log.Write("Error", ex.Message, ex.StackTrace);
                ////				Response.Redirect("errorPages/NoResultsFound.aspx");
                //StatusMessage.Text = "<span class='msgError'>" + ex.Message + "</span>";
            }

            catch (Exception ex)
            {
                if (Session.IsNewSession)
                {
                    user = (User)Session["user"];
                    Session["SessionStartTime"] = DateTime.Now;
                    SessionLog quikLog = new SessionLog(user.ID);
                    quikLog.Write("Info", "Session Timed Out", String.Empty);
                    quikLog.Write("Error", ex.Message, ex.StackTrace);
                    Response.Redirect("UnauthorisedAccessErrorPage.aspx");
                }
                log.Write("Error", ex.Message, ex.StackTrace);
                Response.Redirect("GenericErrorPage.aspx");
            }
        }

        private void SendMail(object mail)
        {
            MailMessage mailMsg = (MailMessage)mail;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("platform.allocation.alerts@gmail.com", "patalerts");
            client.Send(mailMsg);
        }

        private void renderView()
        {
            ShowTeamBoard();
            ShowTechDoc();
            //lblStatus.Text = "";
            refreshGrid();
            CalendarClose.StartDate = DateTime.Now;
            txtBoxSubmitterName.Text = user.Name;
            System.Web.UI.WebControls.Button myButton;
            UpdatePanel9.Visible = false;
            UpdatePanel4.Visible = true;
            lblDeclined.Visible = false;
            txtboxDeclineReasonShow.Visible = false;
            mpeDeclineReason.Hide();
            if (user.IsAdmin)
            {
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeApproved");
                myButton.Visible = false;
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeClaimed");
                myButton.Visible = false;
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnWaitingApproval");
                myButton.Visible = false;
                btnDeclineDemand.Visible = false;
            }
            else
            {
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnNewDemand");
                myButton.Visible = false;
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnSaved");
                myButton.Visible = false;
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnMaintenance");
                myButton.Visible = false;
                txtBoxCloseDateEdit.Enabled = false;
                CalendarCloseEdit.Enabled = false;
                btnDeleteDemand.Visible = false;
                if(user.Role.Equals("TeamMgr"))
                {
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnActive");
                    myButton.Visible = false;
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeClaimed");
                    myButton.Visible = false;
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnWaitingApproval");
                    myButton.Visible = false;
                    

                    //Edit Modal Popup controls
                    txtBoxDemandNameEdit.Enabled = false;
                    dropDownStatusMenuEdit.Enabled = false;
                    DropDownListProgramNameEdit.Enabled = false;
                    DropDownListPlatformNameEdit.Enabled = false;
                    ImageButton3.Visible = false;
                    CalendarCloseEdit.Enabled = false;
                    txtBoxCloseDateEdit.Enabled = false;
                    ImageButton4.Visible = false;
                    btnTeamBoardAddEdit.Attributes.Add("Style", "display:none");
                    btnTechDocEdit.Attributes.Add("Style", "display:none");
                    GridViewTeamBoardsEdit.Columns[0].Visible = false;
                    TechnicalDocGridEdit.Columns[0].Visible = false;

                }
                else if (user.Role.Equals("TeamRep")) 
                {
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnActive");
                    myButton.Visible = false;
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeApproved");
                    myButton.Visible = false;
                    btnDeclineDemand.Visible = false;
                    txtBoxCloseDateEdit.Enabled = false;
                    ImageButton4.Visible = false;
                    btnTeamBoardAddEdit.Attributes.Add("Style", "display:none");
                    btnTechDocEdit.Attributes.Add("Style", "display:none");
                    txtBoxDemandNameEdit.Enabled = false;
                    dropDownStatusMenuEdit.Enabled = false;
                    DropDownListProgramNameEdit.Enabled = false;
                    DropDownListPlatformNameEdit.Enabled = false;
                    ImageButton3.Visible = false;
                }
            }
        }

        private void refreshGrid()
        {
            DataView dv = null;
            if(status.Equals("Open"))
            {
                dv = new DataView(Demand.ListOpenDemands(user));
            }
            else if (status.Equals("Saved"))
            {
                dv = new DataView(Demand.ListSavedDemands(user));
            }
            else if (status.Equals("Approved"))
            {
                dv = new DataView(Demand.ListApprovedDemands(user));
            }
            else if (status.Equals("Closed"))
            {
                dv = new DataView(Demand.ListClosedDemands(user));
            }
            else if (status.Equals("Ordered"))
            {
                dv = new DataView(Demand.ListOrderedDemands(user));
            }
            else if (status.Equals("Declined"))
            {
                dv = new DataView(Demand.ListDeclinedDemands(user));
            }
            DemandsGrid.DataSource = dv;
            DemandsGrid.DataBind();
                      
        }

        public void getDeclinedDemandRecords()
        {
            status = "Declined";
            refreshGrid();
            UpdatePanel9.Visible = false;
            UpdatePanel4.Visible = true;
            lblDeclined.Visible = true;
            txtboxDeclineReasonShow.Visible = true;
            txtboxDeclineReasonShow.Enabled = false;
            String userRole = user.Role;
            if (userRole.Equals("Administrator"))
            {
                txtBoxDemandNameEdit.Enabled = false;
                dropDownStatusMenuEdit.Enabled = false;
                DropDownListProgramNameEdit.Enabled = false;
                DropDownListPlatformNameEdit.Enabled = false;
                ImageButton3.Visible = false;
                ImageButton4.Visible = true;
                CalendarCloseEdit.Enabled = true;
                btnTeamBoardAddEdit.Attributes.Add("Style", "display:none");
                btnTechDocEdit.Attributes.Add("Style", "display:none");
                btnUpdate.Text = "Update";
                GridViewTeamBoardsEdit.Columns[0].Visible = true;
                TechnicalDocGridEdit.Columns[0].Visible = true;
                btnUpdate.Visible = true;
            }
            else if (userRole.Equals("TeamMgr"))
            {
                btnUpdate.Visible = true;
                btnUpdate.Text = "Approve";
                btnDeclineDemand.Visible = false;
            }
            else if (userRole.Equals("TeamRep"))
            {
                GridViewTeamBoardsEdit.Columns[0].Visible = true;
                TechnicalDocGridEdit.Columns[0].Visible = true;
                btnUpdate.Text = "Save";
                btnUpdate.Visible = true;
            }
        }

        public void getOpenDemandRecords()
        {
            status = "Open";
            refreshGrid();
            UpdatePanel9.Visible = false;
            UpdatePanel4.Visible = true;
            lblDeclined.Visible = false;
            txtboxDeclineReasonShow.Visible = false;
            //render Modal controls
            String userRole = user.Role;
            if (userRole.Equals("Administrator")) 
            {
                txtBoxDemandNameEdit.Enabled = true;
                dropDownStatusMenuEdit.Enabled = false;
                DropDownListProgramNameEdit.Enabled = true;
                DropDownListPlatformNameEdit.Enabled = true;
                ImageButton3.Visible = true;
                ImageButton4.Visible = true;
                CalendarClose.Enabled = true;
                btnTeamBoardAddEdit.Attributes.Remove("Style");
                btnTechDocEdit.Attributes.Remove("Style");
                btnUpdate.Text = "Update";
                GridViewTeamBoardsEdit.Columns[0].Visible = true;
                TechnicalDocGridEdit.Columns[0].Visible = true;
                btnUpdate.Visible = true;
            }
            else if (userRole.Equals("TeamMgr"))
            {
                btnDeclineDemand.Visible = false;
            }
            else if (userRole.Equals("TeamRep"))
            {
                GridViewTeamBoardsEdit.Columns[0].Visible = true;
                TechnicalDocGridEdit.Columns[0].Visible = true;
                btnUpdate.Text = "Save";
                btnUpdate.Visible = true;
            }
        }

        public void showNewDemandWindow()
        {
            ShowTeamBoard();
            ShowTechDoc();
            NewDemandModal.Show();
        }

        public void getApprovedDemandRecords()
        {
            status = "Approved";
            refreshGrid();
            UpdatePanel9.Visible = false;
            UpdatePanel4.Visible = true;
            lblDeclined.Visible = false;
            txtboxDeclineReasonShow.Visible = false;
            //render Modal controls
            String userRole = user.Role;
            if (userRole.Equals("Administrator"))
            {
                txtBoxDemandNameEdit.Enabled = false;
                dropDownStatusMenuEdit.Enabled = false;
                DropDownListProgramNameEdit.Enabled = false;
                DropDownListPlatformNameEdit.Enabled = false;
                ImageButton3.Visible = false;
                ImageButton4.Visible = false;
                CalendarCloseEdit.Enabled = false;
                btnTeamBoardAddEdit.Attributes.Add("Style", "display:none");
                btnTechDocEdit.Attributes.Add("Style", "display:none");
                btnUpdate.Text = "Order";
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
                btnUpdate.Visible = true;
            }
            else if (userRole.Equals("TeamMgr"))
            {
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
                btnUpdate.Visible = false;
                btnDeclineDemand.Visible = true;
            }
            else
            {
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
                btnUpdate.Visible = false;
                btnDeclineDemand.Visible = false;
            }
        }

        public void getSavedDemandRecords()
        {
            status = "Saved";
            refreshGrid();
            UpdatePanel9.Visible = false;
            UpdatePanel4.Visible = true;
            lblDeclined.Visible = false;
            txtboxDeclineReasonShow.Visible = false;
            //render Modal controls
            String userRole = user.Role;
            if (userRole.Equals("Administrator"))
            {
                txtBoxDemandNameEdit.Enabled = false;
                dropDownStatusMenuEdit.Enabled = false;
                DropDownListProgramNameEdit.Enabled = false;
                DropDownListPlatformNameEdit.Enabled = false;
                ImageButton3.Visible = false;
                ImageButton4.Visible = true;
                CalendarCloseEdit.Enabled = true;
                btnTeamBoardAddEdit.Attributes.Add("Style", "display:none");
                btnTechDocEdit.Attributes.Add("Style", "display:none");
                btnUpdate.Text = "Update";
                GridViewTeamBoardsEdit.Columns[0].Visible = true;
                TechnicalDocGridEdit.Columns[0].Visible = true;
                btnUpdate.Visible = true;
            }
            else if (userRole.Equals("TeamMgr"))
            {
                btnUpdate.Visible = true;
                btnUpdate.Text = "Approve";
                btnDeclineDemand.Visible = true;
            }
            else if (userRole.Equals("TeamRep"))
            {
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
                btnUpdate.Visible = false;
            }
        }

        public void getClosedDemandRecords()
        {
            status = "Closed";
            refreshGrid();
            UpdatePanel9.Visible = false;
            UpdatePanel4.Visible = true;
            lblDeclined.Visible = false;
            txtboxDeclineReasonShow.Visible = false;
            //render Modal controls
            String userRole = user.Role;
            if (userRole.Equals("Administrator"))
            {
                txtBoxDemandNameEdit.Enabled = false;
                dropDownStatusMenuEdit.Enabled = false;
                DropDownListProgramNameEdit.Enabled = false;
                DropDownListPlatformNameEdit.Enabled = false;
                ImageButton3.Visible = false;
                ImageButton4.Visible = true;
                CalendarCloseEdit.Enabled = true;
                btnTeamBoardAddEdit.Attributes.Add("Style", "display:none");
                btnTechDocEdit.Attributes.Add("Style", "display:none");
                btnUpdate.Text = "Modify Window";
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
                btnUpdate.Visible = true;
            }
            else 
            {
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
                btnUpdate.Visible = false;
            }
        }

        public void getOrderedDemandRecords()
        {
            status = "Ordered";
            refreshGrid();
            UpdatePanel9.Visible = false;
            UpdatePanel4.Visible = true;
            lblDeclined.Visible = false;
            txtboxDeclineReasonShow.Visible = false;
            //render Modal controls
            String userRole = user.Role;
            if (userRole.Equals("Administrator"))
            {
                txtBoxDemandNameEdit.Enabled = false;
                dropDownStatusMenuEdit.Enabled = false;
                DropDownListProgramNameEdit.Enabled = false;
                DropDownListPlatformNameEdit.Enabled = false;
                ImageButton3.Visible = false;
                ImageButton4.Visible = false;
                CalendarCloseEdit.Enabled = false;
                btnTeamBoardAddEdit.Attributes.Add("Style", "display:none");
                btnTechDocEdit.Attributes.Add("Style", "display:none");
                btnUpdate.Visible = false;
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
            }
            else
            {
                GridViewTeamBoardsEdit.Columns[0].Visible = false;
                TechnicalDocGridEdit.Columns[0].Visible = false;
                btnUpdate.Visible = false;
            }
        }

        private void ShowTeamBoard()
        {
            List<TeamBoard> tbList = ViewState["BoardList"] as List<TeamBoard>;
            if (tbList.Count == 0)
            {
                tbListDummy.Clear();
                tbListDummy.Add(new TeamBoard());
                TeamBoardGrid.DataSource = tbListDummy;
                TeamBoardGrid.DataBind();
                TeamBoardGrid.Rows[0].Cells[0].Text = "";
            }
            else
            {
                tbListDummy.Clear();
                TeamBoardGrid.DataSource = tbList;
                TeamBoardGrid.DataBind();
            }
        }

        protected void DemandsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = "Demand ID";
                    e.Row.Cells[1].Text = "Demand Name";
                    e.Row.Cells[2].Text = "Team Name";
                    e.Row.Cells[3].Text = "Platform Name";
                    e.Row.Cells[4].Text = "Program Name";
                    e.Row.Cells[5].Text = "Close Date";
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#EDBBAF'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    if ((e.Row.RowIndex + 2) < 10)
                    {
                        e.Row.Attributes["onclick"] = "javascript:__doPostBack('ctl00$MainContent$DemandsGrid$ctl0" + (e.Row.RowIndex + 2) + "$lblEdit','')";
                    }
                    else
                    {
                        e.Row.Attributes["onclick"] = "javascript:__doPostBack('ctl00$MainContent$DemandsGrid$ctl" + (e.Row.RowIndex + 2) + "$lblEdit','')";
                    }
                     
                    //Page.ClientScript.GetPostBackClientHyperlink((LinkButton)DemandsGrid.Rows[e.Row.RowIndex+2].FindControl("lblEdit"), "");
                    e.Row.Attributes["style"] = "cursor:pointer";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            bool isNew = false;
            String name = String.Empty;
            String pfName = String.Empty;
            String prgName = String.Empty;
            DateTime date = new DateTime();
            if (txtBoxDemandName.Text.Trim().Length == 0)
            {
                errorMsg.Append("Enter Demand Name.<br/>");
                validData = false;
            }
            else
            {
                name = txtBoxDemandName.Text.Trim();
            }
            if (DropDownListProgramName.SelectedValue == "-1")
            {
                errorMsg.Append("Enter Program Name.<br/>");
                validData = false;
            }
            else
            {
                prgName = DropDownListProgramName.SelectedItem.Text;
            }

            if (DropDownListPlatformName.Enabled)
            {
                if (DropDownListPlatformName.SelectedValue == "-1")
                {
                    errorMsg.Append("Enter Platform Name.<br/>");
                    validData = false;
                }
                else
                {
                    pfName = (DropDownListPlatformName.SelectedItem.Text);
                }
            }
            else
            {
                if (txtBoxOtherPlatform.Text.Trim().Length == 0)
                {
                    errorMsg.Append("Enter Platform Name.<br/>");
                    validData = false;
                }
                else
                {
                    pfName = txtBoxOtherPlatform.Text.Trim();
                    isNew = true;
                }
            }

            if (txtBoxCloseDate.Text.Trim().Length == 0)
            {
                errorMsg.Append("Enter Close Date.<br/>");
                validData = false;
            }
            else
            {
                date = System.Convert.ToDateTime(txtBoxCloseDate.Text);
                date = date.Date.Add(new TimeSpan(23, 59, 59));
            }
            List<TeamBoard> tbList = ViewState["BoardList"] as List<TeamBoard>;
            if (tbList.Count == 0)
            {
                errorMsg.Append("Enter board information.<br/>");
                validData = false;
            }
            Boolean noTeamSelected = true;
            List<String> Teams = new List<String>();
            List<TeamBoard> newTbList = new List<TeamBoard>();
            foreach (ListItem liTeams in RadioButtonListTeams.Items)
            {
                if (liTeams.Selected)
                {
                    noTeamSelected = false;
                    if (!Teams.Contains(liTeams.Text))
                        Teams.Add(liTeams.Text);
                    foreach (TeamBoard liTeamBoard in tbList)
                    {
                        newTbList.Add(new TeamBoard(liTeamBoard.SKU, liTeamBoard.BoardType, liTeams.Text));
                    }
                }
            }

            if(noTeamSelected)
            {
                errorMsg.Append("Select team(s).<br/>");
                validData = false;
            }

            List<TechnicalDocumentation> tdList = ViewState["TechDoc"] as List<TechnicalDocumentation>;
            if (validData)
            {
                if (isNew)
                {
                    ConnectionData.WriteNewPlatform(pfName, user);
                    DropDownListPlatformName.DataBind();
                    DropDownListPlatformNameEdit.DataBind();
                }
                Demand demand = new Demand(name, prgName, pfName, date, newTbList, tdList);
                Int16 demandID = ConnectionData.WriteNewDemand(demand, user);
                
               // lblStatus.Text = "Successfully inserted DemandID: " + demandID;
               // MessageBox.Show("Demand ID -- " + demandID + " inserted successfully", "New Demand");
                ClearNewBoardForm();
                NewDemandModal.Hide();
                //DemandsGrid.DataSource = new DataView(Demand.ListAll());
                refreshGrid();
                UpdatePanel4.Update();
                System.Web.UI.WebControls.Button btnNewDemand = (System.Web.UI.WebControls.Button)Master.FindControl("btnNewDemand");
                btnNewDemand.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Demand ID:" + demandID + " inserted successfully')", true);
                try
                {
                    Team t = new Team(Teams.First());

                    MailMessage mail = new MailMessage();
                    mail.To.Add(t.RepEmailId);
                    mail.CC.Add(t.MgrEmailId);
                    mail.CC.Add(user.Email);
                    mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                    mail.Subject = "New demand (#" + demandID + ") has been created";
                    StringBuilder mailBody = new StringBuilder();
                    mailBody.Append("Hi, \n A New demand  has been created for your team and is ready for review.\n");
                    mailBody.Append("\nID: " + demandID);
                    mailBody.Append("\nName: " + demand.DemandName);
                    mailBody.Append("\nProgram Name: " + demand.ProgramName);
                    mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                    mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                    mailBody.Append("\nClose Date: " + demand.CloseDate);
                    mailBody.Append("\nBoards: ");
                    foreach (TeamBoard tb in demand.Boards)
                    {
                        mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                    }
                    mailBody.Append("\nTechnical Documents ");
                    foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                    {
                        mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                    }
                    mail.Body = mailBody.ToString();

                    Thread emailThread = new Thread(SendMail);
                    emailThread.Start(mail);
                   
                }
                catch(Exception ex)
                {

                }
                
                
            }
            else
            {
                NewDemandModal.Show();
                lblNewDemandStatus.ForeColor = System.Drawing.Color.Red;
                lblNewDemandStatus.Text = errorMsg.ToString();
                ShowTeamBoard();
                ShowTechDoc();
            }
        }

        private void ClearNewBoardForm()
        {
            lblNewDemandStatus.Text = "";
            txtBoxDemandName.Text = "";
            DropDownListProgramName.SelectedIndex = -1;
            if (DropDownListPlatformName.Enabled)
            {
                DropDownListPlatformName.SelectedIndex = -1;
            }
            else
            {
                txtBoxOtherPlatform.Text = "";
                txtBoxOtherPlatform.Visible = false;
                DropDownListPlatformName.Enabled = true;
            }
            CalendarClose.SelectedDate = null;
            txtBoxCloseDate.Text = "";
            RadioButtonListTeams.ClearSelection();
//            CheckBoxListTeams.ClearSelection();
            ClearTeamBoard();
            ShowTeamBoard();
            ClearTechDoc();
            ShowTechDoc();
        }

        private void ClearTeamBoard()
        {
            List<TeamBoard> tbList = ViewState["BoardList"] as List<TeamBoard>;
            tbList.Clear();
            ViewState["BoardList"] = tbList;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearNewBoardForm();
            NewDemandModal.Hide();
            System.Web.UI.WebControls.Button btnNewDemand = (System.Web.UI.WebControls.Button)Master.FindControl("btnNewDemand");
            btnNewDemand.Attributes.Add("Style", "background-image: url('../Images/tabunselected.png'); padding-right:30px; padding-left:0px; background-repeat: no-repeat");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            ShowTeamBoard();
            ShowTechDoc();
            if (txtBoxOtherPlatform.Visible)
            {
                txtBoxOtherPlatform.Text = "";
                txtBoxOtherPlatform.Visible = false;
                DropDownListPlatformName.Enabled = true;
            }
            else
            {
                txtBoxOtherPlatform.Visible = true;
                DropDownListPlatformName.Enabled = false;
            }
        }


        protected void btnSaveTeamBoard_Click(object sender, EventArgs e)
        {
            String bt = String.Empty;
            String sku = String.Empty;
            String team = String.Empty;
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            bool isNew = false;
            if (DropDownListBoardType.Enabled)
            {
                if (DropDownListBoardType.SelectedValue == "-1")
                {
                    errorMsg.Append("Enter Board type.<br/>");
                    validData = false;
                }
                else
                {
                    bt = DropDownListBoardType.SelectedItem.Text;
                }
            }
            else
            {
                if (txtboxOtherBoardType.Text == String.Empty)
                {
                    errorMsg.Append("Enter Board type.<br/>");
                    validData = false;
                }
                else
                {
                    bt = txtboxOtherBoardType.Text.Trim();
                    isNew = true;
                }
            }

            if (DropDownListSKU.Enabled)
            {
                if (DropDownListSKU.SelectedValue == "-1")
                {
                    errorMsg.Append("Enter SKU.<br/>");
                    validData = false;
                }
                else
                {
                    sku = DropDownListSKU.SelectedItem.Text;
                }
            }
            else
            {
                if (txtBoxOtherSKU.Text == String.Empty)
                {
                    errorMsg.Append("Enter SKU.<br/>");
                    validData = false;
                }
                else
                {
                    sku = txtBoxOtherSKU.Text.Trim();
                    isNew = true;
                }
            }

            if (validData)
            {
                List<TeamBoard> tbList = ViewState["BoardList"] as List<TeamBoard>;
                if (!tbList.Any(x => x.BoardType == bt && x.SKU == sku))
                {
                    TeamBoard tb = new TeamBoard(sku, bt, null);
                    tbList.Add(tb);
                    mpeTeamBoard.Hide();
                    TeamBoardGrid.DataSource = tbList;
                    TeamBoardGrid.DataBind();
                    clearTeamBoardForm();
                    lblTeamBoardStatus.Text = "";
                    ShowTechDoc();
                    if (isNew)
                    {
                        ConnectionData.WriteNewBoard(tb, user);
                        DropDownListBoardType.DataBind();
                        DropDownListPlatformNameEdit.DataBind();
                    }
                }
                else
                {
                    mpeTeamBoard.Show();
                    lblTeamBoardStatus.ForeColor = System.Drawing.Color.Red;
                    lblTeamBoardStatus.Text = "This board request already exists.";
                }
            }
            else
            {
                mpeTeamBoard.Show();
                lblTeamBoardStatus.ForeColor = System.Drawing.Color.Red;
                lblTeamBoardStatus.Text = errorMsg.ToString();
            }

        }

        protected void btnOtherBoardType_Click(object sender, EventArgs e)
        {
            mpeTeamBoard.Show();
            ShowTeamBoard();
            ShowTechDoc();
            if (txtboxOtherBoardType.Visible)
            {
                DropDownListBoardType.Enabled = true;
                DropDownListSKU.Enabled = true;
                btnOtherSKU.Enabled = true;
                txtboxOtherBoardType.Visible = false;
                txtboxOtherBoardType.Text = "";
                txtBoxOtherSKU.Visible = false;
                txtBoxOtherSKU.Text = "";
            }
            else
            {
                DropDownListBoardType.Enabled = false;
                DropDownListSKU.Enabled = false;
                txtboxOtherBoardType.Visible = true;
                txtBoxOtherSKU.Visible = true;
                btnOtherSKU.Enabled = false;
            }
        }

        protected void btnOtherSKU_Click(object sender, EventArgs e)
        {
            mpeTeamBoard.Show();
            ShowTeamBoard();
            ShowTechDoc();
            if (txtBoxOtherSKU.Visible)
            {
                txtBoxOtherSKU.Text = "";
                txtBoxOtherSKU.Visible = false;
                DropDownListSKU.Enabled = true;
            }
            else
            {
                txtBoxOtherSKU.Visible = true;
                DropDownListSKU.Enabled = false;
            }

        }


        protected void btnCloseTeamBoard_Click(object sender, EventArgs e)
        {
            clearTeamBoardForm();
            ShowTeamBoard();
            ShowTechDoc();
        }

        private void clearTeamBoardForm()
        {
            mpeTeamBoard.Hide();
            //if (DropDownListBoardType.Enabled)
            //{
            //    DropDownListBoardType.SelectedIndex = -1;
            //}
            //else
            //{
            txtboxOtherBoardType.Text = "";
            txtboxOtherBoardType.Visible = false;
            DropDownListBoardType.Enabled = true;
            DropDownListBoardType.SelectedIndex = -1;
            btnOtherBoardType.Enabled = true;
            //}
            //if (DropDownListSKU.Enabled)
            //{
            //    DropDownListSKU.SelectedIndex = -1;
            //}
            //else
            //{
            txtBoxOtherSKU.Text = "";
            txtBoxOtherSKU.Visible = false;
            DropDownListSKU.Enabled = true;
            DropDownListSKU.SelectedIndex = -1;
            btnOtherSKU.Enabled = true;
            //}
            lblTeamBoardStatus.Text = "";
        }

        protected void DropDownListPlatformName_DataBound(object sender, EventArgs e)
        {
            ListItem liPlatform = new ListItem("Select Platform", "-1");
            DropDownListPlatformName.Items.Insert(0, liPlatform);
        }

        protected void DropDownListProgramName_DataBound(object sender, EventArgs e)
        {
            ListItem liProgram = new ListItem("Select Program", "-1");
            DropDownListProgramName.Items.Insert(0, liProgram);
        }

        protected void DropDownListBoardType_DataBound(object sender, EventArgs e)
        {
            ListItem liBoardType = new ListItem("Select Board type", "-1");
            DropDownListBoardType.Items.Insert(0, liBoardType);
        }

        protected void DropDownListSKU_DataBound(object sender, EventArgs e)
        {
            ListItem liSku = new ListItem("Select SKU", "-1");
            DropDownListSKU.Items.Insert(0, liSku);
        }


        protected void DropDownListBoardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            mpeTeamBoard.Show();
            ShowTeamBoard();
            ShowTechDoc();
            DropDownList sku = pnlTeamBoard.FindControl("DropDownListSKU") as DropDownList;
            if (sku.Enabled == false)
            {
                sku.Enabled = true;
                txtBoxOtherSKU.Visible = false;
                txtBoxOtherSKU.Text = "";
            }
        }

   /*     protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList sku = TeamBoardGrid.Rows[TeamBoardGrid.EditIndex].FindControl("DropDownList2") as DropDownList;
            //ListItem liSku = new ListItem("Select SKU", "-1");
            //sku.Items.Insert(0, liSku);
            //sku.SelectedIndex = 0;

        }*/

        protected void TeamBoardGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                TeamBoardGrid.EditIndex = rowIndex;
                ShowTeamBoard();
            }
            else if (e.CommandName == "DeleteRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                List<TeamBoard> tbList = ViewState["BoardList"] as List<TeamBoard>;
                tbList.RemoveAt(rowIndex);
                ShowTeamBoard();
            }
            else if (e.CommandName == "CancelUpdate")
            {
                TeamBoardGrid.EditIndex = -1;
                ShowTeamBoard();
            }
            else if (e.CommandName == "UpdateRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                List<TeamBoard> tbList = ViewState["BoardList"] as List<TeamBoard>;
                tbList[rowIndex].BoardType = ((DropDownList)TeamBoardGrid.Rows[rowIndex].FindControl("ddlBT")).SelectedValue;
                tbList[rowIndex].SKU = ((DropDownList)TeamBoardGrid.Rows[rowIndex].FindControl("lblBT")).SelectedValue;
                TeamBoardGrid.EditIndex = -1;
                ShowTeamBoard();
            }
        }

        protected void btnSaveTechDoc_Click(object sender, EventArgs e)
        {
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            String name = String.Empty;
            String link = String.Empty;
            name = txtboxTechDocName.Text.Trim();
            link = txtBoxTechDocLink.Text.Trim();
            if (name.Length == 0)
            {
                validData = false;
                errorMsg.Append("Enter name for your technical document. <br/>");
            }
            if (link.Length == 0)
            {
                validData = false;
                errorMsg.Append("Enter link for your technical document.<br/>");
            }
            if (validData)
            {
                List<TechnicalDocumentation> tdList = ViewState["TechDoc"] as List<TechnicalDocumentation>;
                TechnicalDocumentation td = new TechnicalDocumentation(name, link);
                if (!tdList.Any(x => x.TDocName == name && x.TDocAddress == link))
                {
                    tdList.Add(td);
                    mpeTechDoc.Hide();
                    TechnicalDocGrid.DataSource = tdList;
                    TechnicalDocGrid.DataBind();
                    ShowTeamBoard();
                    clearTechDocForm();
                }
                else
                {
                    mpeTechDoc.Show();
                    lblTechDocStatus.ForeColor = System.Drawing.Color.Red;
                    errorMsg.Append("This technical doc entry already exists.<br/>");
                    lblTechDocStatus.Text = errorMsg.ToString();
                    validData = false;
                }
            }
            else
            {
                mpeTechDoc.Show();
                lblTechDocStatus.ForeColor = System.Drawing.Color.Red;
                lblTechDocStatus.Text = errorMsg.ToString();
            }
        }

        protected void btnCloseTechDoc_Click(object sender, EventArgs e)
        {
            mpeTechDoc.Hide();
            clearTechDocForm();
            ShowTeamBoard();
            ShowTechDoc();
        }
        private void ClearTechDoc()
        {
            List<TechnicalDocumentation> tdList = new List<TechnicalDocumentation>();
            tdList.Clear();
            ViewState["TechDoc"] = tdList;
        }

        private void clearTechDocForm()
        {
            lblTechDocStatus.Text = "";
            txtboxTechDocName.Text = "";
            txtBoxTechDocLink.Text = "";
        }

        private void ShowTechDoc()
        {
            List<TechnicalDocumentation> tdList = ViewState["TechDoc"] as List<TechnicalDocumentation>;
            if (tdList.Count == 0)
            {
                tdListDummy.Add(new TechnicalDocumentation());
                TechnicalDocGrid.DataSource = tdListDummy;
                TechnicalDocGrid.DataBind();
                TechnicalDocGrid.Rows[0].Cells[0].Text = "";
                TechnicalDocGrid.Rows[0].Cells[1].Text = "";
            }
            else
            {
                tdListDummy.Clear();
                TechnicalDocGrid.DataSource = tdList;
                TechnicalDocGrid.DataBind();
            }
        }

        protected void TechnicalDocGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                TeamBoardGrid.EditIndex = rowIndex;
                ShowTeamBoard();
            }
            else if (e.CommandName == "DeleteRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                List<TechnicalDocumentation> tdList = ViewState["TechDoc"] as List<TechnicalDocumentation>;
                tdList.RemoveAt(rowIndex);
                ShowTechDoc();
            }
            else if (e.CommandName == "CancelUpdate")
            {
                TechnicalDocGrid.EditIndex = -1;
                ShowTechDoc();
            }
        }

        protected void DemandsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditThisRow")
            {
                Int16 demandId = Convert.ToInt16(e.CommandArgument);
                renderDemandView(demandId);
                UpdatePanel4.Update();
                mpeEdit.Show();

            }
        }

        #region EditFunctionality
        private void renderDemandView(Int16 demandId)
        {
            demand = new Demand(demandId);
            txtBoxDemandNameEdit.Text = demand.DemandName;
            dropDownStatusMenuEdit.SelectedValue = demand.Status;
            txtBoxSubmitterNameEdit.Text = user.Name;
            DropDownListProgramNameEdit.SelectedValue = demand.ProgramName;
            DropDownListPlatformNameEdit.SelectedValue = demand.PlatformName;
            txtBoxOpenDateEdit.Text = demand.CreatedDate.Date.ToString("MM-dd-yyyy");
            txtBoxCloseDateEdit.Text = demand.CloseDate.Date.ToString("MM-dd-yyyy");
            CalendarCloseEdit.StartDate = DateTime.Now;
            GridViewTeamBoardsEdit.DataSource = demand.Boards;
            GridViewTeamBoardsEdit.DataBind();
            RadioButtonListTeamsEdit.SelectedValue = demand.Boards[0].TeamName;
            ShowTechDocForEdit();
            txtboxDeclineReasonShow.Text = demand.DeclineReason;
        }

        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            ClearEditForm();
            mpeEdit.Hide();
           // demand = null;
        }


        protected void GridViewTeamBoardsEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                if (GridViewTeamBoardsEdit.EditIndex != -1)
                {
                    String bType = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[GridViewTeamBoardsEdit.EditIndex].FindControl("lblBTEdit")).Text;
                    String sku = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[GridViewTeamBoardsEdit.EditIndex].FindControl("lblSKUEdit")).Text;
                    Int16 noOfBoards = Convert.ToInt16(((System.Web.UI.WebControls.TextBox)GridViewTeamBoardsEdit.Rows[GridViewTeamBoardsEdit.EditIndex].FindControl("txtNoOfBoardsEdit")).Text);
                    List<TeamBoard> tbList = demand.Boards;
                    foreach (TeamBoard tb in tbList)
                    {
                        if (tb.BoardType.Equals(bType) && tb.SKU.Equals(sku))
                        {
                            tb.NumberOfBoards = noOfBoards;
                            break;
                        }
                    }
                }
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                GridViewTeamBoardsEdit.EditIndex = rowIndex;
                mpeEdit.Show();
                ShowTeamBoardForEdit();
            }
            else if (e.CommandName == "DeleteRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                String bType = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[rowIndex].FindControl("lblBTEdit")).Text;
                String sku = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[rowIndex].FindControl("lblSKUEdit")).Text;
                List<TeamBoard> tbList = demand.Boards;
                foreach (TeamBoard tb in tbList )
                {
                    if (tb.BoardType.Equals(bType) && tb.SKU.Equals(sku))
                    {
                        tbList.Remove(tb);
                        break;
                    }
                }
                mpeEdit.Show();
                ShowTeamBoardForEdit();
            }
            else if (e.CommandName == "CancelUpdate")
            {
                GridViewTeamBoardsEdit.EditIndex = -1;
                mpeEdit.Show();
                ShowTeamBoardForEdit();
            }
            
        }

        private void ShowTeamBoardForEdit()
        {
            List<TeamBoard> tbList = demand.Boards;
            if (tbList.Count == 0)
            {
                tbListDummy.Clear();
                tbListDummy.Add(new TeamBoard());
                GridViewTeamBoardsEdit.DataSource = tbListDummy;
                GridViewTeamBoardsEdit.DataBind();
                GridViewTeamBoardsEdit.Rows[0].Cells[0].Text = "";
            }
            else
            {
                tbListDummy.Clear();
                GridViewTeamBoardsEdit.DataSource = tbList;
                GridViewTeamBoardsEdit.DataBind();
            }
        }

        private void ShowTechDocForEdit()
        {
            List<TechnicalDocumentation> tdList = demand.TechnicalDocumentation;
            if (tdList.Count == 0)
            {
                tdListDummy.Add(new TechnicalDocumentation());
                TechnicalDocGridEdit.DataSource = tdListDummy;
                TechnicalDocGridEdit.DataBind();
                TechnicalDocGridEdit.Rows[0].Cells[0].Text = "";
                TechnicalDocGridEdit.Rows[0].Cells[1].Text = "";
            }
            else
            {
                TechnicalDocGridEdit.DataSource = tdList;
                TechnicalDocGridEdit.DataBind();
            }
        }

        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            ShowTechDocForEdit();
            if (txtBoxOtherPlatformEdit.Visible)
            {
                txtBoxOtherPlatformEdit.Text = "";
                txtBoxOtherPlatformEdit.Visible = false;
                DropDownListPlatformNameEdit.Enabled = true;
            }
            else
            {
                txtBoxOtherPlatformEdit.Visible = true;
                DropDownListPlatformNameEdit.Enabled = false;
            }
            mpeEdit.Show();
        }

        protected void TechnicalDocGridEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                TechnicalDocGridEdit.EditIndex = rowIndex;
                mpeEdit.Show();
                ShowTeamBoardForEdit();
            }
            else if (e.CommandName == "DeleteRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                String lName = ((System.Web.UI.WebControls.HyperLink)TechnicalDocGridEdit.Rows[rowIndex].FindControl("HyperLink1")).Text;
                List<TechnicalDocumentation> tdList = demand.TechnicalDocumentation;
                foreach(TechnicalDocumentation td in tdList)
                {
                    if(td.TDocName.Equals(lName))
                    {
                        tdList.Remove(td);
                    }
                }
                mpeEdit.Show();
                ShowTechDocForEdit();
            }
            else if (e.CommandName == "CancelUpdate")
            {
                TechnicalDocGrid.EditIndex = -1;
                mpeEdit.Show();
                ShowTechDoc();
            }
        }

        private void updateOpenDemand()
        {
            StringBuilder errorMsg = new StringBuilder();                 
            bool validData = true;
            if (user.Role.Equals("Administrator"))
            {

                bool isNew = false;
                String name = String.Empty;
                String pfName = String.Empty;
                String prgName = String.Empty;
                DateTime date = new DateTime();
                if (txtBoxDemandNameEdit.Text.Trim().Length == 0)
                {
                    errorMsg.Append("Enter Demand Name.<br/>");
                    validData = false;
                }
                else
                {
                    demand.DemandName = txtBoxDemandNameEdit.Text.Trim();
                }
                //To-Do: Check for number of boards
                demand.Status = dropDownStatusMenuEdit.SelectedValue;
                if (DropDownListProgramNameEdit.SelectedValue == "-1")
                {
                    errorMsg.Append("Enter Program Name.<br/>");
                    validData = false;
                }
                else
                {
                    demand.ProgramName = DropDownListProgramNameEdit.SelectedItem.Text;
                }

                if (DropDownListPlatformNameEdit.Enabled)
                {
                    if (DropDownListPlatformNameEdit.SelectedValue == "-1")
                    {
                        errorMsg.Append("Enter Platform Name.<br/>");
                        validData = false;
                    }
                    else
                    {
                        demand.PlatformName = (DropDownListPlatformNameEdit.SelectedItem.Text);
                    }
                }
                else
                {
                    if (txtBoxOtherPlatformEdit.Text.Trim().Length == 0)
                    {
                        errorMsg.Append("Enter Platform Name.<br/>");
                        validData = false;
                    }
                    else
                    {
                        demand.PlatformName = txtBoxOtherPlatformEdit.Text.Trim();
                        isNew = true;
                    }
                }

                if (txtBoxCloseDateEdit.Text.Trim().Length == 0)
                {
                    errorMsg.Append("Enter Close Date.<br/>");
                    validData = false;
                }
                else
                {
                    date = System.Convert.ToDateTime(txtBoxCloseDateEdit.Text);
                    date = date.Date.Add(new TimeSpan(23, 59, 59));
                    demand.CloseDate = date;
                }
                List<TeamBoard> tbList = demand.Boards;
                if (tbList.Count == 0)
                {
                    errorMsg.Append("Enter board information.<br/>");
                    validData = false;
                }
                String team = RadioButtonListTeams.SelectedValue;
                if (validData)
                {
                    if (isNew)
                    {
                        ConnectionData.WriteNewPlatform(demand.PlatformName, user);
                    }
                    ConnectionData.EditDemand(demand, user);

                    //Mail the update
                    try
                    {
                        Team t = new Team(demand.Boards.First().TeamName);

                        MailMessage mail = new MailMessage();
                        mail.To.Add(t.RepEmailId);
                        mail.CC.Add(t.MgrEmailId);
                        mail.CC.Add(user.Email);
                        mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                        mail.Subject = "Demand (#" + demand.DemandId + ") has been updated";
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append("Hi, \n The below demand of your team has been updated.\n");
                        mailBody.Append("\nID: " + demand.DemandId);
                        mailBody.Append("\nName: " + demand.DemandName);
                        mailBody.Append("\nProgram Name: " + demand.ProgramName);
                        mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                        mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                        mailBody.Append("\nClose Date: " + demand.CloseDate);
                        mailBody.Append("\nBoards: ");
                        foreach (TeamBoard tb in demand.Boards)
                        {
                            mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                        }
                        mailBody.Append("\nTechnical Documents ");
                        foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                        {
                            mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                        }
                        mail.Body = mailBody.ToString();

                        Thread emailThread = new Thread(SendMail);
                        emailThread.Start(mail);

                    }
                    catch (Exception ex)
                    {

                    }


                    //  lblStatus.Text = "Successfully edited DemandID: " + demand.DemandId;
                    ClearEditForm();
                    mpeEdit.Hide();
                    refreshGrid();
                    DropDownListPlatformName.DataBind();
                    DropDownListPlatformNameEdit.DataBind();
                    UpdatePanel4.Update();
                    UpdatePanel1.Update();
                }
                else
                {
                    mpeEdit.Show();
                    lblEditDemandStatusEdit.ForeColor = System.Drawing.Color.Red;
                    lblEditDemandStatusEdit.Text = errorMsg.ToString();
                    ShowTeamBoardForEdit();
                    ShowTechDocForEdit();
                }
            }
            else if (user.Role.Equals("TeamRep"))
            {
                bool allBoardsSaved = true;
                List<TeamBoard> tbList = demand.Boards;
                if (tbList.Count == 0)
                {
                    errorMsg.Append("Enter board information.<br/>");
                    validData = false;
                }
                else
                {
                    foreach (TeamBoard tb in tbList)
                    {
                        if (tb.NumberOfBoards < 1)
                        {
                            allBoardsSaved = false;
                        }
                    }
                    if (allBoardsSaved == true)
                    {
                        demand.Status = "Saved";
                    }
                }
                if (validData && allBoardsSaved)
                {
                    ConnectionData.SaveDemand(demand, user);

                    //Mail the saved demand
                    try
                    {
                        Team t = new Team(demand.Boards.First().TeamName);

                        MailMessage mail = new MailMessage();
                        mail.CC.Add(t.RepEmailId);
                        mail.To.Add(t.MgrEmailId);
                        mail.CC.Add(ConnectionData.GetAdmin().Email);    
                        mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                        mail.Subject = "Demand (#" + demand.DemandId + ") awaits approval";
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append("Hi, \n The below demand of your team has been saved by "+t.RepId+". Please review it.\n");
                        mailBody.Append("\nID: " + demand.DemandId);
                        mailBody.Append("\nName: " + demand.DemandName);
                        mailBody.Append("\nProgram Name: " + demand.ProgramName);
                        mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                        mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                        mailBody.Append("\nClose Date: " + demand.CloseDate);
                        mailBody.Append("\nBoards: ");
                        foreach (TeamBoard tb in demand.Boards)
                        {
                            mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                        }
                        mailBody.Append("\nTechnical Documents ");
                        foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                        {
                            mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                        }
                        mail.Body = mailBody.ToString();

                        Thread emailThread = new Thread(SendMail);
                        emailThread.Start(mail);

                    }
                    catch (Exception ex)
                    {

                    }
                    ClearEditForm();
                    mpeEdit.Hide();
                    refreshGrid();
                    UpdatePanel4.Update();
                    UpdatePanel1.Update();
                }
                else if (validData) 
                {
                    ConnectionData.EditTeamBoard(demand);
                    ClearEditForm();
                    mpeEdit.Hide();
                    refreshGrid();
                    UpdatePanel4.Update();
                    UpdatePanel1.Update();
                }
                else
                {
                    mpeEdit.Show();
                    lblEditDemandStatusEdit.ForeColor = System.Drawing.Color.Red;
                    lblEditDemandStatusEdit.Text = errorMsg.ToString();
                    ShowTeamBoardForEdit();
                    ShowTechDocForEdit();
                }
            }
        }
        private void updateApprovedDemand()
        {
            demand.Status = "Ordered";
            ConnectionData.UpdateDemandStatus(demand, user);
            ClearEditForm();
            mpeEdit.Hide();
            refreshGrid();
            UpdatePanel4.Update();
            UpdatePanel1.Update();
            //Mail the saved demand
            try
            {
                Team t = new Team(demand.Boards.First().TeamName);

                MailMessage mail = new MailMessage();
                mail.To.Add(t.RepEmailId);
                mail.To.Add(t.MgrEmailId);
                mail.CC.Add(ConnectionData.GetAdmin().Email);
                mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                mail.Subject = "Demand (#" + demand.DemandId + ") has been ordered";
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("Hi, \n The below demand of your team has been ordered.\n");
                mailBody.Append("\nID: " + demand.DemandId);
                mailBody.Append("\nName: " + demand.DemandName);
                mailBody.Append("\nProgram Name: " + demand.ProgramName);
                mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                mailBody.Append("\nClose Date: " + demand.CloseDate);
                mailBody.Append("\nBoards: ");
                foreach (TeamBoard tb in demand.Boards)
                {
                    mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                }
                mailBody.Append("\nTechnical Documents ");
                foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                {
                    mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                }
                mail.Body = mailBody.ToString();

                Thread emailThread = new Thread(SendMail);
                emailThread.Start(mail);

            }
            catch (Exception ex)
            {

            }
           
        }

        private void updateClosedDemand()
        {
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            DateTime date = new DateTime();
            
            if (txtBoxCloseDateEdit.Text.Trim().Length == 0)
            {
                errorMsg.Append("Enter Close Date.<br/>");
                validData = false;
            }
            else
            {
                date = System.Convert.ToDateTime(txtBoxCloseDateEdit.Text);              
                date = date.Date.Add(new TimeSpan(23, 59, 59));
                demand.CloseDate = date;
                if (demand.CloseDate >= System.DateTime.Today)
                {
                    demand.Status = "Open";
                }
            }

            if (validData)
            {
                ConnectionData.EditDemandDetails(demand, user);
                //Mail the extended demand
                try
                {
                    Team t = new Team(demand.Boards.First().TeamName);

                    MailMessage mail = new MailMessage();
                    mail.To.Add(t.RepEmailId);
                    mail.CC.Add(t.MgrEmailId);
                    mail.CC.Add(ConnectionData.GetAdmin().Email);
                    mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                    mail.Subject = "Demand (#" + demand.DemandId + ") has an extended close date";
                    StringBuilder mailBody = new StringBuilder();
                    mailBody.Append("Hi, \n The below demand window of your team has been extended. Please review it.\n");
                    mailBody.Append("\nID: " + demand.DemandId);
                    mailBody.Append("\nName: " + demand.DemandName);
                    mailBody.Append("\nProgram Name: " + demand.ProgramName);
                    mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                    mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                    mailBody.Append("\nClose Date: " + demand.CloseDate);
                    mailBody.Append("\nBoards: ");
                    foreach (TeamBoard tb in demand.Boards)
                    {
                        mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                    }
                    mailBody.Append("\nTechnical Documents ");
                    foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                    {
                        mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                    }
                    mail.Body = mailBody.ToString();

                    Thread emailThread = new Thread(SendMail);
                    emailThread.Start(mail);

                }
                catch (Exception ex)
                {

                }
                ClearEditForm();
                mpeEdit.Hide();
                refreshGrid();
                UpdatePanel4.Update();
                UpdatePanel1.Update();
            }
            else
            {
                mpeEdit.Show();
                lblEditDemandStatusEdit.ForeColor = System.Drawing.Color.Red;
                lblEditDemandStatusEdit.Text = errorMsg.ToString();
                ShowTeamBoardForEdit();
                ShowTechDocForEdit();
            }
        }

        private void updateSavedDemand()
        {
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            
            bool allBoardsSaved = true;
            List<TeamBoard> tbList = demand.Boards;
            if (tbList.Count == 0)
            {
                errorMsg.Append("Enter board information.<br/>");
                btnTeamBoardAddEdit.Attributes.Remove("Style");
                validData = false;
            }
            else
            {
                foreach (TeamBoard tb in tbList)
                {
                    if (tb.NumberOfBoards < 1)
                    {
                        allBoardsSaved = false;
                    }
                }
                if (allBoardsSaved == true)
                {
                    if (user.Role.Equals("Administrator"))
                    {
                        demand.Status = "Saved";
                    }
                    else if(user.Role.Equals("TeamMgr"))
                    {
                        demand.Status = "Approved";
                    }
                }
            }
            if (validData && allBoardsSaved)
            {
                ConnectionData.SaveDemand(demand, user);
                if (demand.Status.Equals("Saved"))
                {
                    //Mail the saved demand
                    try
                    {
                        Team t = new Team(demand.Boards.First().TeamName);

                        MailMessage mail = new MailMessage();
                        mail.CC.Add(t.RepEmailId);
                        mail.To.Add(t.MgrEmailId);
                        mail.CC.Add(ConnectionData.GetAdmin().Email);
                        mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                        mail.Subject = "Demand (#" + demand.DemandId + ") awaits approval";
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append("Hi, \n The below demand of your team has been saved by " + t.RepId + ". Please review it.\n");
                        mailBody.Append("\nID: " + demand.DemandId);
                        mailBody.Append("\nName: " + demand.DemandName);
                        mailBody.Append("\nProgram Name: " + demand.ProgramName);
                        mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                        mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                        mailBody.Append("\nClose Date: " + demand.CloseDate);
                        mailBody.Append("\nBoards: ");
                        foreach (TeamBoard tb in demand.Boards)
                        {
                            mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                        }
                        mailBody.Append("\nTechnical Documents ");
                        foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                        {
                            mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                        }
                        mail.Body = mailBody.ToString();

                        Thread emailThread = new Thread(SendMail);
                        emailThread.Start(mail);

                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (demand.Status.Equals("Approved"))
                {
                    //Mail the saved demand
                    try
                    {
                        Team t = new Team(demand.Boards.First().TeamName);

                        MailMessage mail = new MailMessage();
                        mail.CC.Add(t.RepEmailId);
                        mail.CC.Add(t.MgrEmailId);
                        mail.To.Add(ConnectionData.GetAdmin().Email);
                        mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                        mail.Subject = "Demand (#" + demand.DemandId + ") has been approved";
                        StringBuilder mailBody = new StringBuilder();
                        mailBody.Append("Hi, \n The below demand of your team has been approved by " + t.MgrId + " and is ready for ordering.\n");
                        mailBody.Append("\nID: " + demand.DemandId);
                        mailBody.Append("\nName: " + demand.DemandName);
                        mailBody.Append("\nProgram Name: " + demand.ProgramName);
                        mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                        mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                        mailBody.Append("\nClose Date: " + demand.CloseDate);
                        mailBody.Append("\nBoards: ");
                        foreach (TeamBoard tb in demand.Boards)
                        {
                            mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                        }
                        mailBody.Append("\nTechnical Documents ");
                        foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                        {
                            mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                        }
                        mail.Body = mailBody.ToString();

                        Thread emailThread = new Thread(SendMail);
                        emailThread.Start(mail);

                    }
                    catch (Exception ex)
                    {

                    }
                }
                
                ClearEditForm();
                mpeEdit.Hide();
                refreshGrid();
                UpdatePanel4.Update();
                UpdatePanel1.Update();
            }
            else if (validData)
            {
                ConnectionData.EditTeamBoard(demand);
                //Mail the saved demand
                try
                {
                    Team t = new Team(demand.Boards.First().TeamName);

                    MailMessage mail = new MailMessage();
                    mail.To.Add(t.RepEmailId);
                    mail.CC.Add(t.MgrEmailId);
                    mail.CC.Add(ConnectionData.GetAdmin().Email);
                    mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                    mail.Subject = "Demand (#" + demand.DemandId + ") has been updated";
                    StringBuilder mailBody = new StringBuilder();
                    mailBody.Append("Hi, \n The below demand has been updated by " + t.RepId + ".\n");
                    mailBody.Append("\nID: " + demand.DemandId);
                    mailBody.Append("\nName: " + demand.DemandName);
                    mailBody.Append("\nProgram Name: " + demand.ProgramName);
                    mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                    mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                    mailBody.Append("\nClose Date: " + demand.CloseDate);
                    mailBody.Append("\nBoards: ");
                    foreach (TeamBoard tb in demand.Boards)
                    {
                        mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                    }
                    mailBody.Append("\nTechnical Documents ");
                    foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                    {
                        mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                    }
                    mail.Body = mailBody.ToString();

                    Thread emailThread = new Thread(SendMail);
                    emailThread.Start(mail);

                }
                catch (Exception ex)
                {

                }
                ClearEditForm();
                mpeEdit.Hide();
                refreshGrid();
                UpdatePanel4.Update();
                UpdatePanel1.Update();
            }
            else
            {
                mpeEdit.Show();
                lblEditDemandStatusEdit.ForeColor = System.Drawing.Color.Red;
                lblEditDemandStatusEdit.Text = errorMsg.ToString();
                ShowTeamBoardForEdit();
                ShowTechDocForEdit();
            }            
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (GridViewTeamBoardsEdit.EditIndex != -1)
            {
                if (GridViewTeamBoardsEdit.EditIndex != -1)
                {
                    String bType = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[GridViewTeamBoardsEdit.EditIndex].FindControl("lblBTEdit")).Text;
                    String sku = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[GridViewTeamBoardsEdit.EditIndex].FindControl("lblSKUEdit")).Text;
                    Int16 noOfBoards = Convert.ToInt16(((System.Web.UI.WebControls.TextBox)GridViewTeamBoardsEdit.Rows[GridViewTeamBoardsEdit.EditIndex].FindControl("txtNoOfBoardsEdit")).Text);
                    List<TeamBoard> tbList = demand.Boards;
                    foreach (TeamBoard tb in tbList)
                    {
                        if (tb.BoardType.Equals(bType) && tb.SKU.Equals(sku))
                        {
                            tb.NumberOfBoards = noOfBoards;
                            break;
                        }
                    }
                }
                /*
                for (int rowIndex = 0; rowIndex < GridViewTeamBoardsEdit.Rows.Count; rowIndex++)
                {
                    String bType = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[rowIndex].FindControl("lblBTEdit")).Text;
                    String sku = ((System.Web.UI.WebControls.Label)GridViewTeamBoardsEdit.Rows[rowIndex].FindControl("lblSKUEdit")).Text;
                    Int16 noOfBoards = Convert.ToInt16(((System.Web.UI.WebControls.TextBox)GridViewTeamBoardsEdit.Rows[rowIndex].FindControl("txtNoOfBoardsEdit")).Text);
                    List<TeamBoard> tbList = demand.Boards;
                    foreach (TeamBoard tb in tbList)
                    {
                        if (tb.BoardType.Equals(bType) && tb.SKU.Equals(sku))
                        {
                            tb.NumberOfBoards = noOfBoards;
                            break;
                        }
                    }
                }*/
                GridViewTeamBoardsEdit.EditIndex = -1;
            }
            if (status.Equals("Open"))
            {
                updateOpenDemand();
            }
            else if (status.Equals("Saved") || status.Equals("Declined"))
            {
                updateSavedDemand();
            }
            else if (status.Equals("Approved"))
            {
                updateApprovedDemand();
            }
            else if (status.Equals("Closed"))
            {
                updateClosedDemand();
            }
            
        }
        private void ClearEditForm()
        {
            lblEditDemandStatusEdit.Text = "";
            if (!DropDownListPlatformNameEdit.Enabled && user.Role.Equals("Administration"))
            {
                txtBoxOtherPlatformEdit.Text = "";
                txtBoxOtherPlatformEdit.Visible = false;
                DropDownListPlatformNameEdit.Enabled = true;
            }
            ShowTeamBoardForEdit();
            ShowTechDocForEdit();
        }

        protected void DemandsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int16 demandId = Convert.ToInt16(e.CommandArgument);
            int rowIndex = DemandsGrid.SelectedRow.RowIndex;
            Int16 demandId = Convert.ToInt16(((System.Web.UI.WebControls.LinkButton)DemandsGrid.Rows[rowIndex].FindControl("lblEdit")).Text);
            renderDemandView(demandId);
            UpdatePanel4.Update();
            mpeEdit.Show();
        }

        protected void DropDownListBoardTypeEdit_DataBound(object sender, EventArgs e)
        {
            ListItem liBoardType = new ListItem("Select Board type", "-1");
            DropDownListBoardTypeEdit.Items.Insert(0, liBoardType);
        }

        protected void DropDownListBoardTypeEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeTeamBoardEdit.Show();
            ShowTeamBoardForEdit();
            ShowTechDocForEdit();
            DropDownList sku = pnlTeamBoardEdit.FindControl("DropDownListSKUEdit") as DropDownList;
            if (DropDownListSKUEdit.Enabled == false)
            {
                DropDownListSKUEdit.Enabled = true;
                txtBoxOtherSKUEdit.Visible = false;
                txtBoxOtherSKUEdit.Text = "";
            }
        }

        protected void btnOtherBoardTypeEdit_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeTeamBoardEdit.Show();
            ShowTeamBoardForEdit();
            ShowTechDocForEdit();
            if (txtboxOtherBoardTypeEdit.Visible)
            {
                DropDownListBoardTypeEdit.Enabled = true;
                DropDownListSKUEdit.Enabled = true;
                btnOtherSKUEdit.Enabled = true;
                txtboxOtherBoardTypeEdit.Visible = false;
                txtboxOtherBoardTypeEdit.Text = "";
                txtBoxOtherSKUEdit.Visible = false;
                txtBoxOtherSKUEdit.Text = "";
            }
            else
            {
                DropDownListBoardTypeEdit.Enabled = false;
                DropDownListSKUEdit.Enabled = false;
                txtboxOtherBoardTypeEdit.Visible = true;
                txtBoxOtherSKUEdit.Visible = true;
                btnOtherSKUEdit.Enabled = false;
            }
        }

        protected void DropDownListSKUEdit_DataBound(object sender, EventArgs e)
        {
            ListItem liSku = new ListItem("Select SKU", "-1");
            DropDownListSKUEdit.Items.Insert(0, liSku);
        }

        protected void btnOtherSKUEdit_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeTeamBoardEdit.Show();
            ShowTeamBoardForEdit();
            ShowTechDocForEdit();
            if (txtBoxOtherSKUEdit.Visible)
            {
                txtBoxOtherSKUEdit.Text = "";
                txtBoxOtherSKUEdit.Visible = false;
                DropDownListSKUEdit.Enabled = true;
            }
            else
            {
                txtBoxOtherSKUEdit.Visible = true;
                DropDownListSKUEdit.Enabled = false;
            }
        }

        protected void btnSaveTeamBoardEdit_Click(object sender, EventArgs e)
        {
            String bt = String.Empty;
            String sku = String.Empty;
            String team = String.Empty;
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            bool isNew = false;
            if (DropDownListBoardTypeEdit.Enabled)
            {
                if (DropDownListBoardTypeEdit.SelectedValue == "-1")
                {
                    errorMsg.Append("Enter Board type.<br/>");
                    validData = false;
                }
                else
                {
                    bt = DropDownListBoardTypeEdit.SelectedItem.Text;
                }
            }
            else
            {
                if (txtboxOtherBoardTypeEdit.Text == String.Empty)
                {
                    errorMsg.Append("Enter Board type.<br/>");
                    validData = false;
                }
                else
                {
                    bt = txtboxOtherBoardTypeEdit.Text.Trim();
                    isNew = true;
                }
            }

            if (DropDownListSKUEdit.Enabled)
            {
                if (DropDownListSKUEdit.SelectedValue == "-1")
                {
                    errorMsg.Append("Enter SKU.<br/>");
                    validData = false;
                }
                else
                {
                    sku = DropDownListSKUEdit.SelectedItem.Text;
                }
            }
            else
            {
                if (txtBoxOtherSKUEdit.Text == String.Empty)
                {
                    errorMsg.Append("Enter SKU.<br/>");
                    validData = false;
                }
                else
                {
                    sku = txtBoxOtherSKUEdit.Text.Trim();
                    isNew = true;
                }
            }

            if (validData)
            {
                List<TeamBoard> tbList = demand.Boards;
                if (!tbList.Any(x => x.BoardType == bt && x.SKU == sku))
                {
                    TeamBoard tb = new TeamBoard(sku, bt, RadioButtonListTeamsEdit.SelectedValue);
                    tbList.Add(tb);
                    mpeTeamBoardEdit.Hide();
                    mpeEdit.Show();
                    GridViewTeamBoardsEdit.DataSource = tbList;
                    GridViewTeamBoardsEdit.DataBind();
                    clearTeamBoardFormThroughEdit();
                    lblTeamBoardStatusEdit.Text = "";
                    ShowTechDocForEdit();
                    if (isNew)
                    {
                        ConnectionData.WriteNewBoard(tb, user);
                        DropDownListBoardType.DataBind();
                    }
                }
                else
                {
                    mpeEdit.Show();
                    mpeTeamBoardEdit.Show();
                    lblTeamBoardStatusEdit.ForeColor = System.Drawing.Color.Red;
                    lblTeamBoardStatusEdit.Text = "This board request already exists.";
                }
            }
            else
            {
                mpeEdit.Show();
                mpeTeamBoardEdit.Show();
                lblTeamBoardStatusEdit.ForeColor = System.Drawing.Color.Red;
                lblTeamBoardStatusEdit.Text = errorMsg.ToString();
            }

        }

        private void clearTeamBoardFormThroughEdit()
        {
            mpeTeamBoardEdit.Hide();
            txtboxOtherBoardTypeEdit.Text = "";
            txtboxOtherBoardTypeEdit.Visible = false;
            DropDownListBoardTypeEdit.Enabled = true;
            DropDownListBoardTypeEdit.SelectedIndex = -1;
            btnOtherBoardTypeEdit.Enabled = true;
            txtBoxOtherSKUEdit.Text = "";
            txtBoxOtherSKUEdit.Visible = false;
            DropDownListSKUEdit.Enabled = true;
            DropDownListSKUEdit.SelectedIndex = -1;
            btnOtherSKUEdit.Enabled = true;
            lblTeamBoardStatusEdit.Text = "";
        }

        protected void btnCloseTeamBoardEdit_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            clearTeamBoardFormThroughEdit();
            ShowTeamBoardForEdit();
            ShowTechDocForEdit();
        }

        protected void btnSaveTechDocEdit_Click(object sender, EventArgs e)
        {
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            String name = String.Empty;
            String link = String.Empty;
            name = txtboxTechDocNameEdit.Text.Trim();
            link = txtBoxTechDocLinkEdit.Text.Trim();
            if (name.Length == 0)
            {
                validData = false;
                errorMsg.Append("Enter name for your technical document. <br/>");
            }
            if (link.Length == 0)
            {
                validData = false;
                errorMsg.Append("Enter link for your technical document.<br/>");
            }
            if (validData)
            {
                List<TechnicalDocumentation> tdList = demand.TechnicalDocumentation;
                TechnicalDocumentation td = new TechnicalDocumentation(name, link);
                if (!tdList.Any(x => x.TDocName == name && x.TDocAddress == link))
                {
                    tdList.Add(td);
                    mpeTechDocEdit.Hide();
                    mpeEdit.Show();
                    TechnicalDocGridEdit.DataSource = tdList;
                    TechnicalDocGridEdit.DataBind();
                    ShowTeamBoardForEdit();
                    clearTechDocFormThroughEdit();
                }
                else
                {
                    mpeEdit.Show();
                    mpeTechDocEdit.Show();
                    lblTechDocStatusEdit.ForeColor = System.Drawing.Color.Red;
                    errorMsg.Append("This technical doc entry already exists.<br/>");
                    lblTechDocStatusEdit.Text = errorMsg.ToString();
                    validData = false;
                }
            }
            else
            {
                mpeEdit.Show();
                mpeTechDocEdit.Show();
                lblTechDocStatusEdit.ForeColor = System.Drawing.Color.Red;
                lblTechDocStatusEdit.Text = errorMsg.ToString();
            }
        }
        private void clearTechDocFormThroughEdit()
        {
            lblTechDocStatusEdit.Text = "";
            txtboxTechDocNameEdit.Text = "";
            txtBoxTechDocLinkEdit.Text = "";
        }

        protected void btnCloseTechDocEdit_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeTechDocEdit.Hide();
            clearTechDocFormThroughEdit();
            ShowTeamBoardForEdit();
            ShowTechDocForEdit();
        }

        protected void btnDeleteDemand_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeDeleteConfirm.Show();
            pnlDemandDelete.Visible = true;
        }

        

        #endregion

        #region Session Logout
        public void LogOut()
        {
            status = "";
        }
        #endregion

        #region Admin Utility

        public void AdminUtility()
        {
            UpdatePanel4.Visible = false;
            UpdatePanel9.Visible = true;
        }
        protected void btnSaveNewProgram_Click(object sender, EventArgs e)
        {
            bool validData = true;
            StringBuilder errorMsg = new StringBuilder();
            if (txtboxNewProgramName.Text.Trim().Length == 0)
            {
                errorMsg.Append("Enter Program Name.<br/>");
                validData = false;
            }
            if (validData)
            {
                ConnectionData.AddNewProgram(txtboxNewProgramName.Text.Trim());
                mpeNewProgram.Hide();
                if (DropDownListProgramName.Enabled == true)
                {
                    DropDownListProgramName.DataBind();
                }
                else
                {
                    DropDownListProgramName.DataBind();
                    DropDownListProgramName.Enabled = false;
                }
                if (DropDownListProgramNameEdit.Enabled == true)
                {
                    DropDownListProgramNameEdit.DataBind();
                }
                else
                {
                    DropDownListProgramNameEdit.DataBind();
                    DropDownListProgramNameEdit.Enabled = false;
                }
                
            }
            else
            {
                mpeNewProgram.Show();
                lblNewProgramStatus.ForeColor = System.Drawing.Color.Red;
                lblNewProgramStatus.Text = errorMsg.ToString();
            }
        }

        protected void btnCloseNewProgram_Click(object sender, EventArgs e)
        {
            mpeNewProgram.Hide();
            lblNewProgramStatus.Text = "";
            txtboxNewProgramName.Text = "";
        }

        protected void btnSaveNewPlatform_Click(object sender, EventArgs e)
        {
            bool validData = true;
            StringBuilder errorMsg = new StringBuilder();
            if (txtboxNewPlatformName.Text.Trim().Length == 0)
            {
                errorMsg.Append("Enter Platform Name.<br/>");
                validData = false;
            }
            if (validData)
            {
                ConnectionData.WriteNewPlatform(txtboxNewPlatformName.Text.Trim(), user);
                mpeNewPlatform.Hide();
                if (DropDownListPlatformName.Enabled == true)
                {
                    DropDownListPlatformName.DataBind();
                }
                else
                {
                    DropDownListPlatformName.DataBind();
                    DropDownListPlatformName.Enabled = false;
                }
                if (DropDownListPlatformNameEdit.Enabled == true)
                {
                    DropDownListPlatformNameEdit.DataBind();
                }
                else
                {
                    DropDownListPlatformNameEdit.DataBind();
                    DropDownListPlatformNameEdit.Enabled = false;
                }
                lblNewPlatformStatus.Text = "";
                txtboxNewPlatformName.Text = "";

            }
            else
            {
                mpeNewPlatform.Show();
                lblNewPlatformStatus.ForeColor = System.Drawing.Color.Red;
                lblNewPlatformStatus.Text = errorMsg.ToString();
            }

        }

        protected void btnCloseNewPlatform_Click(object sender, EventArgs e)
        {
            mpeNewPlatform.Hide();
            lblNewPlatformStatus.Text = "";
            txtboxNewPlatformName.Text = "";
        }

        protected void ddlNewBoardSKUBTList_DataBound(object sender, EventArgs e)
        {
            ListItem liBoardType = new ListItem("Select Board type", "-1");
            ddlNewBoardSKUBTList.Items.Insert(0, liBoardType);
        }

        protected void ddlNewBoardSKUBTList_SelectedIndexChanged(object sender, EventArgs e)
        {
            mpeNewBoardSKU.Show();
            txtBoxNewBoardSKUSKU.Text = "";
        }

        protected void bnNewBoardSKUOtherBT_Click(object sender, EventArgs e)
        {
            mpeNewBoardSKU.Show();
            if (txtBoxNewBoardSKUOtherBT.Visible)
            {
                ddlNewBoardSKUBTList.Enabled = true;
                txtBoxNewBoardSKUOtherBT.Text = "";
                txtBoxNewBoardSKUOtherBT.Visible = false;
                txtBoxNewBoardSKUSKU.Text = "";
            }
            else
            {
                ddlNewBoardSKUBTList.Enabled = false;
                txtBoxNewBoardSKUOtherBT.Visible = true;
                txtBoxNewBoardSKUOtherBT.Visible = true;
                txtBoxNewBoardSKUSKU.Text = "";
            }
        }

        protected void btnCloseNewBoardSKU_Click(object sender, EventArgs e)
        {
            mpeNewBoardSKU.Show();
            clearNewBoardSKUForm();
        }

        protected void btnSaveNewBoardSKU_Click(object sender, EventArgs e)
        {
            String bt = String.Empty;
            String sku = String.Empty;
            String team = String.Empty;
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
            if (ddlNewBoardSKUBTList.Enabled)
            {
                if (ddlNewBoardSKUBTList.SelectedValue == "-1")
                {
                    errorMsg.Append("Enter Board type.<br/>");
                    validData = false;
                }
                else
                {
                    bt = ddlNewBoardSKUBTList.SelectedItem.Text;
                }
            }
            else
            {
                if (txtBoxNewBoardSKUOtherBT.Text == String.Empty)
                {
                    errorMsg.Append("Enter Board type.<br/>");
                    validData = false;
                }
                else
                {
                    bt = txtBoxNewBoardSKUOtherBT.Text.Trim();
                }
            }        
            if (txtBoxNewBoardSKUSKU.Text == String.Empty)
            {
                errorMsg.Append("Enter SKU.<br/>");
                validData = false;
            }
            else
            {
                sku = txtBoxNewBoardSKUSKU.Text.Trim();
            }
            
            if (validData)
            {
                mpeNewBoardSKU.Hide();
                clearNewBoardSKUForm();
                ConnectionData.WriteNewBoard(new TeamBoard(sku, bt, null), user);
                if (ddlNewBoardSKUBTList.Enabled)
                {
                    ddlNewBoardSKUBTList.DataBind();
                }
                else
                {
                    ddlNewBoardSKUBTList.DataBind();
                    ddlNewBoardSKUBTList.Enabled = false;
                }
                if (DropDownListBoardType.Enabled)
                {
                    DropDownListBoardType.DataBind();
                }
                else
                {
                    DropDownListBoardType.DataBind();
                    DropDownListBoardType.Enabled = false;
                }
                if (DropDownListPlatformNameEdit.Enabled)
                {
                    DropDownListPlatformNameEdit.DataBind();
                }
                else
                {
                    DropDownListPlatformNameEdit.DataBind();
                    DropDownListPlatformNameEdit.Enabled = false;
                }
            }
            else
            {
                lblNewNBoardSKUStatus.ForeColor = System.Drawing.Color.Red;
                lblNewNBoardSKUStatus.Text = errorMsg.ToString();
                mpeNewBoardSKU.Show();               
            }
       }

        private void clearNewBoardSKUForm()
        {
            mpeNewBoardSKU.Hide();
            txtBoxNewBoardSKUOtherBT.Text = "";
            txtBoxNewBoardSKUOtherBT.Visible = false;
            ddlNewBoardSKUBTList.Enabled = true;
            ddlNewBoardSKUBTList.SelectedIndex = -1;
            ddlNewBoardSKUBTList.Enabled = true;
            txtBoxNewBoardSKUSKU.Text = "";
            lblNewNBoardSKUStatus.Text = "";
        }

        protected void btnDeclineDemand_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeDeclineReason.Show();
            pnlDemandDecline.Visible = true;
        }


        protected void btnSaveDeclineReason_Click(object sender, EventArgs e)
        {
            if (txtboxDeclineReason.Text.Trim().Equals(""))
            {
                mpeEdit.Show();
                mpeDeclineReason.Show();
                lblDeclineStatus.ForeColor = System.Drawing.Color.Red;
                lblDeclineStatus.Text = "Please enter the reason!";
            }
            else
            {
                mpeDeclineReason.Hide();
                mpeEdit.Hide();
                demand.Status = "Declined";
                demand.DeclineReason = txtboxDeclineReason.Text.Trim();
                ConnectionData.UpdateDemandStatus(demand, user);
                ConnectionData.WriteDeclineReason(demand);
                txtboxDeclineReason.Text = "";
                refreshGrid();
                //Mail the declined demand
                try
                {
                    Team t = new Team(demand.Boards.First().TeamName);

                    MailMessage mail = new MailMessage();
                    mail.To.Add(t.RepEmailId);
                    mail.CC.Add(t.MgrEmailId);
                    mail.CC.Add(ConnectionData.GetAdmin().Email);
                    mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                    mail.Subject = "Demand (#" + demand.DemandId + ") has been declined";
                    StringBuilder mailBody = new StringBuilder();
                    mailBody.Append("Hi, \n The below demand of your team has been declined.\n");
                    mailBody.Append("\nID: " + demand.DemandId);
                    mailBody.Append("\nName: " + demand.DemandName);
                    mailBody.Append("\nProgram Name: " + demand.ProgramName);
                    mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                    mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                    mailBody.Append("\nClose Date: " + demand.CloseDate);
                    mailBody.Append("\nBoards: ");
                    foreach (TeamBoard tb in demand.Boards)
                    {
                        mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                    }
                    mailBody.Append("\nTechnical Documents ");
                    foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                    {
                        mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                    }
                    mailBody.Append("\n\nDecline Reason: " + demand.DeclineReason);
                    mail.Body = mailBody.ToString();

                    Thread emailThread = new Thread(SendMail);
                    emailThread.Start(mail);

                }
                catch (Exception ex)
                {

                }
            }
            
            // UpdatePanel4.Update();
        }

        protected void btnCloseDeclineReason_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeDeclineReason.Hide();
            txtboxDeclineReason.Text = "";
            lblDeclineStatus.Text = "";
        }

        protected void btnCloseDeleteConfirm_Click(object sender, EventArgs e)
        {
            mpeEdit.Show();
            mpeDeleteConfirm.Hide();
        }


        protected void btnSaveDeleteConfirm_Click(object sender, EventArgs e)
        {
            ConnectionData.DeleteDemand(demand);
            refreshGrid();
            //Mail the deleted demand
            try
            {
                Team t = new Team(demand.Boards.First().TeamName);

                MailMessage mail = new MailMessage();
                mail.To.Add(t.RepEmailId);
                mail.To.Add(t.MgrEmailId);
                String adminEmail = ConnectionData.GetAdmin().Email;
                mail.CC.Add(adminEmail);
                mail.From = new MailAddress("platform.allocation.alerts@gmail.com");
                mail.Subject = "Demand (#" + demand.DemandId + ") has been deleted";
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("Hi, \n The below demand of your team has been deleted. Please contact the admin(" + adminEmail + ") for details. \n");
                mailBody.Append("\nID: " + demand.DemandId);
                mailBody.Append("\nName: " + demand.DemandName);
                mailBody.Append("\nProgram Name: " + demand.ProgramName);
                mailBody.Append("\nPlatform Name: " + demand.PlatformName);
                mailBody.Append("\nOpen Date: " + demand.CreatedDate);
                mailBody.Append("\nClose Date: " + demand.CloseDate);
                mailBody.Append("\nBoards: ");
                foreach (TeamBoard tb in demand.Boards)
                {
                    mailBody.Append("\n\t" + tb.BoardType + "\t" + tb.SKU);
                }
                mailBody.Append("\nTechnical Documents ");
                foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                {
                    mailBody.Append("\n\t" + td.TDocName + "\t" + td.TDocAddress);
                }
                mail.Body = mailBody.ToString();

                Thread emailThread = new Thread(SendMail);
                emailThread.Start(mail);

            }
            catch (Exception ex)
            {

            }
            // UpdatePanel4.Update();
        }
        #endregion

        
    }
}
        