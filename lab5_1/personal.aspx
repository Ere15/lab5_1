<%@ Page Title="Личный кабинет" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="personal.aspx.cs" Inherits="lab5_1.personal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Личный кабинет</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Личный кабинет</h2>

    <div class="user-info">
        <asp:Label ID="LabelUsername" runat="server" Text="Логин:"></asp:Label>
        <asp:TextBox ID="TextBoxUsername" runat="server" ReadOnly="true"></asp:TextBox><br />


        <asp:Label ID="LabelEmail" runat="server" Text="Электронная почта:"></asp:Label>
        <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox><br /><br />

        <asp:Button ID="ButtonSave" runat="server" Text="Сохранить изменения" OnClick="ButtonSave_Click" />
        <asp:Label ID="LabelError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        <asp:Label ID="LabelSuccess" runat="server" ForeColor="Green" Visible="false"></asp:Label>

    </div>
</asp:Content>
