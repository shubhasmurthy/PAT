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
using Platform_Allocation_Tool.Business_Layer;
using Platform_Allocation_Tool.Data_Layer;

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
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user"] == null)
                Response.Redirect("Login.aspx");

            try
            {
                user = (User)Session["user"];
                log = (SessionLog)Session["log"];

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
                    
                    refreshGrid();
                    renderView();
                    ShowTeamBoard();
                    ShowTechDoc();
                    lblStatus.Text = "";
                    CalendarClose.StartDate = DateTime.Now;
                    txtBoxSubmitterName.Text = user.Name;
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

        private void renderView()
        {
            System.Web.UI.WebControls.Button myButton;
            if (user.IsAdmin)
            {
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeApproved");
                myButton.Visible = false;
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeClaimed");
                myButton.Visible = false;
            }
            else
            {
                myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnNewDemand");
                myButton.Visible = false;
                if(user.Role.Equals("TeamMgr"))
                {
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnActive");
                    myButton.Visible = false;
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeClaimed");
                    myButton.Visible = false;
                }
                else if (user.Role.Equals("TeamRep")) 
                {
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnActive");
                    myButton.Visible = false;
                    myButton = (System.Web.UI.WebControls.Button)Master.FindControl("btnToBeApproved");
                    myButton.Visible = false;
                }
                NewDemandModalPanel.Visible = false;
                EditDemandModalPanel.Visible = false;
            }
            

        }

        private void refreshGrid()
        {

            DataView dv = new DataView(Demand.ListAll(user));              
            DemandsGrid.DataSource = dv;
            DemandsGrid.DataBind();         
        }

        public void getOpenDemandRecords()
        {
            DataView dv = new DataView(Demand.ListOpenDemands(user));
            DemandsGrid.DataSource = dv;
            DemandsGrid.DataBind();
        }

        public void getApprovedDemandRecords()
        {
            DataView dv = new DataView(Demand.ListApprovedDemands(user));
            DemandsGrid.DataSource = dv;
            DemandsGrid.DataBind();
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
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Demand ID";
                e.Row.Cells[1].Text = "Demand Name";
                e.Row.Cells[2].Text = "Platform Name";
                e.Row.Cells[3].Text = "Program Name";
                e.Row.Cells[4].Text = "Close Date";
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
                lblStatus.Text = "Successfully inserted DemandID: " + demandID;
               // MessageBox.Show("Demand ID -- " + demandID + " inserted successfully", "New Demand");
                ClearNewBoardForm();
                NewDemandModal.Hide();
                //DemandsGrid.DataSource = new DataView(Demand.ListAll());
                refreshGrid();
                UpdatePanel4.Update();

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Demand ID:" + demandID + " inserted successfully')", true);
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
                TeamBoardGrid.EditIndex = -1;
                mpeEdit.Show();
                ShowTeamBoardForEdit();
            }
            else if (e.CommandName == "UpdateRow")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            StringBuilder errorMsg = new StringBuilder();
            bool validData = true;
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
                lblStatus.Text = "Successfully edited DemandID: " + demand.DemandId;
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
        private void ClearEditForm()
        {
            lblEditDemandStatusEdit.Text = "";
            if (!DropDownListPlatformNameEdit.Enabled)
            {
                txtBoxOtherPlatformEdit.Text = "";
                txtBoxOtherPlatformEdit.Visible = false;
                DropDownListPlatformNameEdit.Enabled = true;
            }
            ShowTeamBoardForEdit();
            ShowTechDocForEdit();
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
            ConnectionData.DeleteDemand(demand);
            refreshGrid();
           // UpdatePanel4.Update();
        }

        #endregion
    }
}
        