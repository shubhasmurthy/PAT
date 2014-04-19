<%@ Page Title="" Language="C#" MasterPageFile="~/UI Layer/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Platform_Allocation_Tool.Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Content/GridView.css" rel="stylesheet" type="text/css" />
    <link href="../Content/ModalPopup.css" rel="stylesheet" runat="server" />
    <style type="text/css">
   .wrapper {
   width: 100%;
    }
    .first {
       float: left;
       width: 330px;
    }
    .second {
    float: left;
       width: 235px;
    }
    .third {
       float:left;
       width: 195px;
    }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   

    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <ajaxToolkit:ModalPopupExtender ID="NewDemandModal" runat="server" TargetControlId="btnNewDemand1" PopupControlID="NewDemandModalPanel" OkControlID="btnMFinish" CancelControlID="btnMClose" DropShadow="true" BackgroundCssClass="ModalBackground" />
    <asp:Button ID="btnNewDemand1" runat="server" Style="display: none;" />
    <asp:Button ID="btnMClose" runat="server" Style="display: none;" />
    <asp:Button ID="btnMFinish" runat="server" Style="display: none;" />
    <asp:Panel ID="NewDemandModalPanel" runat="server" CssClass="modalPopup" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:SqlDataSource ID="SqlDataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [Id], [Name] FROM [State]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceProgramName" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [Name] FROM [Program]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourcePlatform" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [Name] FROM [Platform]"></asp:SqlDataSource>
 
           <div class="header" style="text-align:center;">
               New Demand
            </div>
        <div style="padding:10px;">
                   <div>
                       <asp:Label ID="lblNewDemandStatus" runat="server" Text=""></asp:Label>
                   </div> 
         <div class="wrapper">
            <div class="first">
	            <div>Demand Name </div>
                <div><asp:TextBox ID="txtBoxDemandName" runat="server" Width="275px"></asp:TextBox></div>
                </div>
            <div class="second">
	            <div>Status</div>
	            <div><asp:DropDownList ID="dropDownStatusMenu" runat="server" Width="170px" DataSourceID="SqlDataSourceStatus" DataTextField="Name" DataValueField="Id" Enabled="False"></asp:DropDownList></div>
            </div>
            <div class="third">
                <div>Submitter</div>
                <div><asp:TextBox ID="txtBoxSubmitterName" runat="server" Enabled="False" Width="160px"></asp:TextBox></div>
            </div>
         </div>
        <br />
        <br />
        <br />
         <div class="wrapper">
            <div class="first">
	            <div>Program Name </div>
                <div><asp:DropDownList ID="DropDownListProgramName" runat="server" Width="170px" DataSourceID="SqlDataSourceProgramName" DataTextField="Name" DataValueField="Name" OnDataBound="DropDownListProgramName_DataBound"></asp:DropDownList></div>
                </div>
            <div class="second">
	            <div>Platform Name</div>
                    <div style="float: left; display: inline;">
                        <asp:DropDownList ID="DropDownListPlatformName" runat="server" Width="170px" DataSourceID="SqlDataSourcePlatform" DataTextField="Name" DataValueField="Name" OnDataBound="DropDownListPlatformName_DataBound" >
                        </asp:DropDownList>
                    </div>
                <div style="float:left; display: inline; padding-left: 3px;">
                        <asp:ImageButton ID="ImageButton2" runat="server" Height="24px" ImageUrl="~/Images/add.png" OnClick="ImageButton2_Click" Width="25px" />
                </div>
                      <div><br />
                       <asp:TextBox ID="txtBoxOtherPlatform" runat="server" Height="16px" Width="170px" Visible="False"></asp:TextBox>
					</div>
			</div>
            <div class="third">
                <div>Close Date</div>
                <div style="float: left; display: inline;"><asp:TextBox ID="txtBoxCloseDate" runat="server" Height="19px" Width="160px"></asp:TextBox></div>
                  <div style="float:left; display: inline; padding-left: 3px;"><asp:ImageButton ID="ImageButton1" runat="server" Height="24px" ImageUrl="~/Images/calendar.png" Width="25px" /></div>
                <ajaxToolkit:CalendarExtender ID="CalendarClose" runat="server" TargetControlID="txtBoxCloseDate" PopupButtonID="ImageButton1" CssClass=".cal_Calendar"></ajaxToolkit:CalendarExtender>
				</div>   
            </div>
            <br />
            <br />
            <br />
            <br />
            <div>
                <div style="float:left; display:inline; width:560px;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                           <ContentTemplate>
                               Items New Demand
                       <asp:GridView ID="TeamBoardGrid" runat="server" CssClass="grid" emptydatatext="Add team boards" AutoGenerateColumns="False" OnRowCommand="TeamBoardGrid_RowCommand" >
                           <AlternatingRowStyle CssClass="gridAlternate" />
                           <RowStyle CssClass="gridItem" Height="22px" />
							<HeaderStyle CssClass="gridHeaderLeft" ForeColor="#FFFFFF" Wrap="false" />
							<FooterStyle CssClass="gridFooter" ForeColor="#FFFFFF" />
                           <Columns>
                                <asp:TemplateField ItemStyle-CssClass="gridItemCenter" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
									<ItemStyle HorizontalAlign="Right"/>
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" CommandName="EditRow" ForeColor="#8C4510" runat="server">Edit</asp:LinkButton>
                                    <asp:LinkButton ID="lbDelete" CommandName="DeleteRow" ForeColor="#8C4510" runat="server" CausesValidation="false">Delete</asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbUpdate" CommandName="UpdateRow" ForeColor="#8C4510" runat="server">Update</asp:LinkButton>
                                    <asp:LinkButton ID="lbCancel" CommandName="CancelUpdate" ForeColor="#8C4510" runat="server" CausesValidation="false">Cancel</asp:LinkButton>
                                </EditItemTemplate>
                                </asp:TemplateField>                           
                                <asp:TemplateField HeaderText="Board Type" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="2%">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlBT" runat="server" SelectedValue='<%# Bind("BoardType") %>' DataSourceID="SqlDataSourceBoardTypes" DataTextField="TypeName" DataValueField="TypeName" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBT" runat="server" Text='<%# Bind("BoardType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SKU" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="2%">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
                                    <EditItemTemplate>
                                <asp:SqlDataSource ID="SqlDataSourceGridViewSKU" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [SKU] FROM [Board] WHERE ([TypeName] = @TypeName)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddBT" Name="TypeName" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                        <asp:DropDownList ID="ddlSKU" runat="server" DataSourceID="SqlDataSourceGridViewSKU" DataTextField="SKU" DataValueField="SKU">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSKU" runat="server" Text='<%# Eval("SKU") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                           </Columns>
                       </asp:GridView>
                               </ContentTemplate>
                           </asp:UpdatePanel>
                    <div style="width:500px; text-align:right;"><asp:Button ID="btnTeamBoardAdd" runat="server" Text="Add" /></div>
                </div>
                <div>
                <div>Teams</div>
                    <asp:RadioButtonList ID="RadioButtonListTeams" runat="server" DataSourceID="SqlDataSourceTeamName" DataTextField="Name" DataValueField="Name"></asp:RadioButtonList>
                    <asp:SqlDataSource ID="SqlDataSourceTeamName" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [Name] FROM [Team]"></asp:SqlDataSource>
            </div>
             </div>
            
            <br />
            <div style="width:560px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        Technical Documentation
                <asp:GridView ID="TechnicalDocGrid" runat="server" CssClass="grid" AutoGenerateColumns="False" OnRowCommand="TechnicalDocGrid_RowCommand" >                   
                            <AlternatingRowStyle CssClass="gridAlternate" />
                           <RowStyle CssClass="gridItem" Height="22px" />
							<HeaderStyle CssClass="gridHeaderLeft" ForeColor="#FFFFFF" Wrap="false" />
							<FooterStyle CssClass="gridFooter" ForeColor="#FFFFFF" />
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="gridItemCenter" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
								<ItemStyle HorizontalAlign="Right"/>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDelete" CommandName="DeleteRow" ForeColor="#8C4510" runat="server" CausesValidation="false">Delete</asp:LinkButton>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
								<ItemStyle HorizontalAlign="Right"/>
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
								<ItemStyle HorizontalAlign="Right"/>
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Bind("TDocName") %>' NavigateUrl = '<%# Bind("TDocAddress") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <div style="width:500px; text-align:right;"><asp:Button ID="btnTechDoc" runat="server" Text="Add" /></div>
            </div>
         </div>

        <div class="footer">
            <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="yes" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="no"  OnClick="btnCancel_Click" />
        </div>
            <ajaxToolkit:ModalPopupExtender ID="mpeTeamBoard" runat="server" TargetControlID="btnTeamBoardAdd" PopupControlID="pnlTeamBoard" OkControlID="btnFinish" CancelControlID="btnClose" DropShadow="true" BackgroundCssClass="ModalBackground"/>
            <asp:Button ID="btnFinish" runat="server" Style="visibility: hidden" />
            <asp:Button ID="btnClose" runat="server" Style="visibility: hidden" />
                <asp:Panel ID="pnlTeamBoard" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
                <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTeamBoardStatus" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Board Type
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListBoardType" runat="server" DataSourceID="SqlDataSourceBoardTypes" DataTextField="TypeName" DataValueField="TypeName" OnDataBound="DropDownListBoardType_DataBound" AutoPostBack="True" OnSelectedIndexChanged="DropDownListBoardType_SelectedIndexChanged" Height="24px"></asp:DropDownList>
                                <asp:Button ID="btnOtherBoardType" runat="server" Text="+" OnClick="btnOtherBoardType_Click" />
                                <asp:SqlDataSource ID="SqlDataSourceBoardTypes" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT DISTINCT [TypeName] FROM [Board]"></asp:SqlDataSource>
                            </td>
                            <td>
                                <asp:TextBox ID="txtboxOtherBoardType" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SKU
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListSKU" runat="server" DataSourceID="SqlDataSourceSKU" DataTextField="SKU" DataValueField="SKU" OnDataBound="DropDownListSKU_DataBound"></asp:DropDownList>
                                <asp:Button ID="btnOtherSKU" runat="server" Text="+" OnClick="btnOtherSKU_Click" />
                                <asp:SqlDataSource ID="SqlDataSourceSKU" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [SKU] FROM [Board] WHERE ([TypeName] = @TypeName)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DropDownListBoardType" Name="TypeName" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxOtherSKU" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSaveTeamBoard" runat="server" OnClick="btnSaveTeamBoard_Click" Text="Save" />
                    <asp:Button ID="btnCloseTeamBoard" runat="server" Text="Close" OnClick="btnCloseTeamBoard_Click" />  
                </asp:Panel>  

            <ajaxToolkit:ModalPopupExtender ID="mpeTechDoc" runat="server" TargetControlID="btnTechDoc" PopupControlID="pnlTechDoc" OkControlID="Button1" CancelControlID="Button2" DropShadow="true" BackgroundCssClass="ModalBackground"/>
            <asp:Button ID="Button1" runat="server" Style="visibility: hidden" />
            <asp:Button ID="Button2" runat="server" Style="visibility: hidden" />
            <asp:Panel ID="pnlTechDoc" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
                <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTechDocStatus" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name
                            </td>                            
                            <td>
                                <asp:TextBox ID="txtboxTechDocName" runat="server" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Link
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxTechDocLink" runat="server" Height="16px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSaveTechDoc" runat="server" OnClick="btnSaveTechDoc_Click" Text="Save" />
                    <asp:Button ID="btnCloseTechDoc" runat="server" Text="Close" OnClick="btnCloseTechDoc_Click" />  
            </asp:Panel>
    
    </ContentTemplate>
    </asp:UpdatePanel>
        </asp:Panel>
    
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" style="padding-bottom:200px;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
         <ContentTemplate>
                <asp:GridView ID="DemandsGrid" runat="server" EmptyDataText="No demands!" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeaderWhenEmpty="True" OnRowDataBound="DemandsGrid_RowDataBound" AutoGenerateColumns="False" OnRowCommand="DemandsGrid_RowCommand" OnSelectedIndexChanged="DemandsGrid_SelectedIndexChanged">
                     <Columns>
                <asp:TemplateField HeaderText="Demand ID">
                    <EditItemTemplate>
                        <asp:Label ID="lblDemandID" runat="server" Text='<%# Bind("DemandId") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lblEdit" CommandArgument='<%# Eval("DemandId") %>' CommandName="EditThisRow" runat="server" CausesValidation="false"><%# Eval("DemandId") %></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DemandName" HeaderText="Demand Name" />
                <asp:BoundField DataField="TeamName" HeaderText="Team Name" />
                <asp:BoundField DataField="PlatformName" HeaderText="Platform Name" />
                <asp:BoundField DataField="ProgramName" HeaderText="Program Name" />
                <asp:BoundField DataField="CloseDate" HeaderText="Close Date" />
            </Columns>
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
            
    </asp:GridView>
             <asp:Button ID="btnDummy" runat="server" Style="visibility: hidden" />
              <asp:Button ID="btnMOKEdit" runat="server" Style="visibility: hidden" />
             <asp:Button ID="btnMCancelEdit" runat="server" Style="visibility: hidden" />
             <ajaxToolkit:ModalPopupExtender ID="mpeEdit" runat="server" TargetControlId="btnDummy" PopupControlID="EditDemandModalPanel" OkControlID="btnMOKEdit" CancelControlID="btnMCancelEdit" DropShadow="true" BackgroundCssClass="ModalBackground" />
              </ContentTemplate>
        </asp:UpdatePanel>
         <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">    
            <ContentTemplate>
                <asp:Panel ID="EditDemandModalPanel" runat="server" CssClass="modalPopup">
                     <div class="header" style="text-align:center;">
                         Demand Details - Edit
                     </div>
                    <div style="padding:10px;">
                       <div>
                           <asp:Label ID="lblEditDemandStatusEdit" runat="server" Text=""></asp:Label>
                       </div> 
                       <div class="wrapper">
                        <div class="first">
	                        <div>Demand Name </div>
                            <div><asp:TextBox ID="txtBoxDemandNameEdit" runat="server" Width="275px"></asp:TextBox></div>
                            </div>
                        <div class="second">
	                        <div>Status</div>
	                        <div><asp:DropDownList ID="dropDownStatusMenuEdit" runat="server" Width="170px" DataSourceID="SqlDataSourceStatus" DataTextField="Name" DataValueField="Name"></asp:DropDownList></div>
                        </div>
                        <div class="third">
                            <div>Submitter</div>
                            <div><asp:TextBox ID="txtBoxSubmitterNameEdit" runat="server" Enabled="False" Width="160px"></asp:TextBox></div>
                        </div>
                     </div>
                        <br /> <br /> <br />
                        <div class="wrapper">
                        <div class="first">
	                        <div>Program Name </div>
                            <div><asp:DropDownList ID="DropDownListProgramNameEdit" runat="server" Width="170px" DataSourceID="SqlDataSourceProgramName" DataTextField="Name" DataValueField="Name"></asp:DropDownList></div>
                            </div>
                        <div class="second">
	                        <div>Platform Name</div>
                                <div style="float: left; display: inline;">
                                    <asp:DropDownList ID="DropDownListPlatformNameEdit" runat="server" Width="170px" DataSourceID="SqlDataSourcePlatform" DataTextField="Name" DataValueField="Name" >
                                    </asp:DropDownList>
                                </div>
                            <div style="float:left; display: inline; padding-left: 3px;">
                                    <asp:ImageButton ID="ImageButton3" runat="server" Height="24px" ImageUrl="~/Images/add.png" OnClick="ImageButton3_Click" Width="25px" />
                            </div>
                                  <div><br />
                                   <asp:TextBox ID="txtBoxOtherPlatformEdit" runat="server" Height="16px" Width="170px" Visible="False"></asp:TextBox>
					            </div>
			            </div>
                        <div class="third">
                            <div>Open Date</div>
                            <div style="float: left; display: inline;"><asp:TextBox ID="txtBoxOpenDateEdit" runat="server" Height="19px" Width="160px" Enabled="False" ></asp:TextBox></div>
                            <div>Close Date</div>
                            <div style="float: left; display: inline;"><asp:TextBox ID="txtBoxCloseDateEdit" runat="server" Height="19px" Width="160px"></asp:TextBox></div>
                              <div style="float:left; display: inline; padding-left: 3px;"><asp:ImageButton ID="ImageButton4" runat="server" Height="24px" ImageUrl="~/Images/calendar.png" Width="25px" /></div>
                            <ajaxToolkit:CalendarExtender ID="CalendarCloseEdit" runat="server" TargetControlID="txtBoxCloseDateEdit" PopupButtonID="ImageButton4" CssClass=".cal_Calendar"></ajaxToolkit:CalendarExtender>
				            </div>   
                        </div>
                     
                        <br /> <br /> <br />  <br />
                    <div>
                     <div style="float:left; display:inline; width:560px;">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                           <ContentTemplate>
                              Demand Items 
                            <asp:GridView ID="GridViewTeamBoardsEdit" runat="server" CssClass="grid" AutoGenerateColumns="False" OnRowCommand="GridViewTeamBoardsEdit_RowCommand">
                           <AlternatingRowStyle CssClass="gridAlternate" />
                           <RowStyle CssClass="gridItem" Height="22px" />
							<HeaderStyle CssClass="gridHeaderLeft" ForeColor="#FFFFFF" Wrap="false" />
							<FooterStyle CssClass="gridFooter" ForeColor="#FFFFFF" />
                           <Columns>
                                <asp:TemplateField ItemStyle-CssClass="gridItemCenter" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
									<ItemStyle HorizontalAlign="Right"/>
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lblEdit" CommandName="EditRow" ForeColor="#8C4510" runat="server">Edit</asp:LinkButton>
                                    <asp:LinkButton ID="lblDelete" CommandName="DeleteRow" ForeColor="#8C4510" runat="server" CausesValidation="false">Delete</asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    
                                    <asp:LinkButton ID="lblCancel" CommandName="CancelUpdate" ForeColor="#8C4510" runat="server" CausesValidation="false">Cancel</asp:LinkButton>
                                </EditItemTemplate>
                                </asp:TemplateField>                           
                                <asp:TemplateField HeaderText="Board Type" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="2%">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
                                    <EditItemTemplate>
                                        <asp:Label ID="lblBTEdit" runat="server" Text='<%# Bind("BoardType") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBTEdit" runat="server" Text='<%# Bind("BoardType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SKU" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="2%">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
                                    <EditItemTemplate>
                                        <asp:Label ID="lblSKUEdit" runat="server" Text='<%# Eval("SKU") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSKUEdit" runat="server" Text='<%# Eval("SKU") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="Number of Boards" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="2%">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNoOfBoardsEdit" runat="server" Text='<%# Bind("NumberOfBoards") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoOfBoardsEdit" runat="server" Text='<%# Bind("NumberOfBoards") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                           </Columns>
                       </asp:GridView>
                               </ContentTemplate>
                           </asp:UpdatePanel>
                    <div style="width:500px; text-align:right;"><asp:Button ID="btnTeamBoardAddEdit" runat="server" Text="Add" /></div>
                </div>
                    <div>
                        <br />
                    <div>Teams</div>
                <asp:RadioButtonList ID="RadioButtonListTeamsEdit" runat="server" DataSourceID="SqlDataSourceTeamName" DataTextField="Name" DataValueField="Name" Enabled="false"></asp:RadioButtonList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [Name] FROM [Team]"></asp:SqlDataSource>
                </div>
                </div>
                    <br />
                    <div style="width:560px;">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        Technical Documentation
                <asp:GridView ID="TechnicalDocGridEdit" runat="server" CssClass="grid" AutoGenerateColumns="False" OnRowCommand="TechnicalDocGridEdit_RowCommand">                   
                            <AlternatingRowStyle CssClass="gridAlternate" />
                           <RowStyle CssClass="gridItem" Height="22px" />
							<HeaderStyle CssClass="gridHeaderLeft" ForeColor="#FFFFFF" Wrap="false" />
							<FooterStyle CssClass="gridFooter" ForeColor="#FFFFFF" />
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="gridItemCenter" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
								<ItemStyle HorizontalAlign="Right"/>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDelete" CommandName="DeleteRow" ForeColor="#8C4510" runat="server" CausesValidation="false">Delete</asp:LinkButton>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
								<ItemStyle HorizontalAlign="Right"/>
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link" ItemStyle-CssClass="gridAlphaItem" ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-Wrap="false">
                                <HeaderStyle CssClass="gridHeader" BorderStyle="None" />
								<ItemStyle HorizontalAlign="Right"/>
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Bind("TDocName") %>' NavigateUrl = '<%# Bind("TDocAddress") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <div style="width:500px; text-align:right;"><asp:Button ID="btnTechDocEdit" runat="server" Text="Add" /></div>
            </div>
                        <div style="width:560px;">
                            <asp:Label ID="lblDeclined" runat="server" Text="Reason for decline "></asp:Label>
                        </div>
                        <div style="width:560px;">
                            <asp:TextBox ID="txtboxDeclineReasonShow" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>

                    <div class="leftFooter">
                    <asp:Button ID="btnDeleteDemand" runat="server" Text="Delete" CssClass="yes" OnClick="btnDeleteDemand_Click" />
                    <asp:Button ID="btnDeclineDemand" runat="server" Text="Decline" CssClass="yes" OnClick="btnDeclineDemand_Click"/>
                    </div>

                    <div class="footer">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="yes" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnCancelUpdate" runat="server" Text="Close" CssClass="no" OnClick="btnCancelUpdate_Click" />
                    </div>
                    <ajaxToolkit:ModalPopupExtender ID="mpeTeamBoardEdit" runat="server" TargetControlID="btnTeamBoardAddEdit" PopupControlID="pnlTeamBoardEdit" OkControlID="btnOTbEdit" CancelControlID="btnFTbEdit" DropShadow="true" BackgroundCssClass="ModalBackground"/>
            <asp:Button ID="btnOTbEdit" runat="server" Style="visibility: hidden" />
            <asp:Button ID="btnFTbEdit" runat="server" Style="visibility: hidden" />
                <asp:Panel ID="pnlTeamBoardEdit" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
                <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTeamBoardStatusEdit" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Board Type
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListBoardTypeEdit" runat="server" DataSourceID="SqlDataSourceBoardTypes" DataTextField="TypeName" DataValueField="TypeName" AutoPostBack="True" Height="24px" OnDataBound="DropDownListBoardTypeEdit_DataBound" OnSelectedIndexChanged="DropDownListBoardTypeEdit_SelectedIndexChanged"></asp:DropDownList>
                                <asp:Button ID="btnOtherBoardTypeEdit" runat="server" Text="+" OnClick="btnOtherBoardTypeEdit_Click"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtboxOtherBoardTypeEdit" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SKU
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListSKUEdit" runat="server" DataSourceID="SqlDataSourceSKUEdit" DataTextField="SKU" DataValueField="SKU" OnDataBound="DropDownListSKUEdit_DataBound"></asp:DropDownList>
                                <asp:Button ID="btnOtherSKUEdit" runat="server" Text="+" OnClick="btnOtherSKUEdit_Click"/>
                                <asp:SqlDataSource ID="SqlDataSourceSKUEdit" runat="server" ConnectionString="<%$ ConnectionStrings:PlatformAllocation_preDbConnection %>" SelectCommand="SELECT [SKU] FROM [Board] WHERE ([TypeName] = @TypeName)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DropDownListBoardTypeEdit" Name="TypeName" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxOtherSKUEdit" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSaveTeamBoardEdit" runat="server" Text="Save" OnClick="btnSaveTeamBoardEdit_Click"/>
                    <asp:Button ID="btnCloseTeamBoardEdit" runat="server" Text="Close" OnClick="btnCloseTeamBoardEdit_Click"/>  
                </asp:Panel>  
            <ajaxToolkit:ModalPopupExtender ID="mpeTechDocEdit" runat="server" TargetControlID="btnTechDocEdit" PopupControlID="pnlTechDocEdit" OkControlID="btnOTdEdit" CancelControlID="btnFTdEdit" DropShadow="true" BackgroundCssClass="ModalBackground"/>
            <asp:Button ID="btnOTdEdit" runat="server" Style="visibility: hidden" />
            <asp:Button ID="btnFTdEdit" runat="server" Style="visibility: hidden" />
            <asp:Panel ID="pnlTechDocEdit" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
                <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTechDocStatusEdit" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name
                            </td>                            
                            <td>
                                <asp:TextBox ID="txtboxTechDocNameEdit" runat="server" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Link
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxTechDocLinkEdit" runat="server" Height="16px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSaveTechDocEdit" runat="server" Text="Save" OnClick="btnSaveTechDocEdit_Click"/>
                    <asp:Button ID="btnCloseTechDocEdit" runat="server" Text="Close" OnClick="btnCloseTechDocEdit_Click"/>  
                </asp:Panel>  
            <ajaxToolkit:ModalPopupExtender ID="mpeDeclineReason" runat="server" TargetControlID="btnDDDummy" PopupControlID="pnlDemandDecline" OkControlID="btnDDOk" CancelControlID="btnDDCancel" DropShadow="true" BackgroundCssClass="ModalBackground"/>
            <asp:Button ID="btnDDOk" runat="server" Style="visibility: hidden" />
            <asp:Button ID="btnDDDummy" runat="server" Style="visibility: hidden" />
            <asp:Button ID="btnDDCancel" runat="server" Style="visibility: hidden" />
            <asp:Panel ID="pnlDemandDecline" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
                <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblDeclineStatus" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDecline" runat="server" Text="Reason for Decline"></asp:Label>
                            </td>
                        </tr>
                        <tr>                           
                            <td>
                                 <asp:TextBox ID="txtboxDeclineReason" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>        
                        </tr>
                    </table>
                    <asp:Button ID="btnSaveDeclineReason" runat="server" Text="Save" OnClick="btnSaveDeclineReason_Click"/>
                    <asp:Button ID="btnCloseDeclineReason" runat="server" Text="Cancel" OnClick="btnCloseDeclineReason_Click"/>  
     
                   </asp:Panel>
                     <ajaxToolkit:ModalPopupExtender ID="mpeDeleteConfirm" runat="server" TargetControlID="btnDCDummy" PopupControlID="pnlDemandDelete" OkControlID="btnDCOk" CancelControlID="btnDCCancel" DropShadow="true" BackgroundCssClass="ModalBackground"/>
            <asp:Button ID="btnDCOk" runat="server" Style="visibility: hidden" />
            <asp:Button ID="btnDCDummy" runat="server" Style="visibility: hidden" />
            <asp:Button ID="btnDCCancel" runat="server" Style="visibility: hidden" />
            <asp:Panel ID="pnlDemandDelete" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
                Are you sure you want to delete?
                <br /><br />
                    <asp:Button ID="btnSaveDeleteConfirm" runat="server" Text="Yes" OnClick="btnSaveDeleteConfirm_Click"/>
                    <asp:Button ID="btnCloseDeleteConfirm" runat="server" Text="No" OnClick="btnCloseDeleteConfirm_Click"/>  
     
                   </asp:Panel>
                    </asp:Panel> 
                </ContentTemplate>           
         </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
     
    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">  
    <ContentTemplate>
        <div>
            <asp:LinkButton ID="lBtnNewProgram" ForeColor="#8C4510" runat="server">Add New Program</asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton ID="lBtnNewPlatform" ForeColor="#8C4510" runat="server">Add New Platform</asp:LinkButton>
        </div>
        <div>
            <asp:LinkButton ID="lBtnNewBoardSKU" ForeColor="#8C4510" runat="server">Add Board-SKU</asp:LinkButton>
        </div>
    
    <ajaxToolkit:ModalPopupExtender ID="mpeNewProgram" runat="server" TargetControlID="lBtnNewProgram" PopupControlID="pnlNewProgram" OkControlID="btnNPO" CancelControlID="btnNPC" DropShadow="true" BackgroundCssClass="ModalBackground"/>
    <asp:Button ID="btnNPO" runat="server" Style="visibility: hidden" />
    <asp:Button ID="btnNPC" runat="server" Style="visibility: hidden" />
    <asp:Panel ID="pnlNewProgram" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
        <table>
                <tr>
                    <td>
                        <asp:Label ID="lblNewProgramStatus" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Program Name
                    </td>                            
                    <td>
                        <asp:TextBox ID="txtboxNewProgramName" runat="server" ></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSaveNewProgram" runat="server" Text="Save" OnClick="btnSaveNewProgram_Click"/>
            <asp:Button ID="btnCloseNewProgram" runat="server" Text="Close" OnClick="btnCloseNewProgram_Click"/>  
        </asp:Panel>  

        <ajaxToolkit:ModalPopupExtender ID="mpeNewPlatform" runat="server" TargetControlID="lBtnNewPlatform" PopupControlID="pnlNewPlatform" OkControlID="btnNPO" CancelControlID="btnNPC" DropShadow="true" BackgroundCssClass="ModalBackground"/>
    <asp:Panel ID="pnlNewPlatform" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
        <table>
                <tr>
                    <td>
                        <asp:Label ID="lblNewPlatformStatus" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Platform Name
                    </td>                            
                    <td>
                        <asp:TextBox ID="txtboxNewPlatformName" runat="server" ></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSaveNewPlatform" runat="server" Text="Save" OnClick="btnSaveNewPlatform_Click"/>
            <asp:Button ID="btnCloseNewPlatform" runat="server" Text="Close" OnClick="btnCloseNewPlatform_Click"/>  
        </asp:Panel>  

    <ajaxToolkit:ModalPopupExtender ID="mpeNewBoardSKU" runat="server" TargetControlID="lBtnNewBoardSKU" PopupControlID="pnlNewBoardSKU" OkControlID="btnNPO" CancelControlID="btnNPC" DropShadow="true" BackgroundCssClass="ModalBackground"/>
    <asp:Panel ID="pnlNewBoardSKU" runat="server" Style="width: 100%; border: solid 1px black; height: 100%; background-color: White; margin-left: 10px">
        <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblNewNBoardSKUStatus" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Board Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlNewBoardSKUBTList" runat="server" DataSourceID="SqlDataSourceBoardTypes" DataTextField="TypeName" DataValueField="TypeName" AutoPostBack="True" Height="24px" OnDataBound="ddlNewBoardSKUBTList_DataBound" OnSelectedIndexChanged="ddlNewBoardSKUBTList_SelectedIndexChanged"></asp:DropDownList>
                                <asp:Button ID="bnNewBoardSKUOtherBT" runat="server" Text="+" OnClick="bnNewBoardSKUOtherBT_Click"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxNewBoardSKUOtherBT" runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SKU
                            </td>
                            <td>
                                 <asp:TextBox ID="txtBoxNewBoardSKUSKU" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
            <asp:Button ID="btnSaveNewBoardSKU" runat="server" Text="Save" OnClick="btnSaveNewBoardSKU_Click"/>
            <asp:Button ID="btnCloseNewBoardSKU" runat="server" Text="Close" OnClick="btnCloseNewBoardSKU_Click"/>  
        </asp:Panel>  
    </ContentTemplate>     
    </asp:UpdatePanel>  
</asp:Content>
