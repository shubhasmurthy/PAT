<%@ Page Title="" Language="C#" MasterPageFile="~/UI Layer/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Platform_Allocation_Tool.UI_Layer.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Name:
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
    </p>
    <p>
        Password:
        <asp:TextBox ID="txtPwd" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" />
    </p>
</asp:Content>
