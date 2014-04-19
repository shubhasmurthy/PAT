<%@ Page Title="" Language="C#" MasterPageFile="~/UI Layer/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Platform_Allocation_Tool.UI_Layer.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container bootshape">
     <div class="simple-login">
	    <h1>WELCOME TO PA TOOL !</h1>
         <h2>ACCOUNT LOGIN</h2>
	    <asp:TextBox ID="txtName" runat="server" placeholder="Username" autofocus required  class="form-control" ></asp:TextBox>
	    <asp:TextBox ID="txtPwd" runat="server" required placeholder="Password" class="form-control"></asp:TextBox>
	    <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" class="btn btn-lg btn-primary btn-block"/>
     </div>
</div>
</asp:Content>

