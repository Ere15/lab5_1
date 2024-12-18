<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reg.aspx.cs" Inherits="lab5_1.reg" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Регистрация</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Регистрация</h2>
    <p>Заполните форму ниже, чтобы создать учетную запись.</p>

    <asp:Panel ID="RegistrationPanel" runat="server">
        <asp:Label ID="LabelUsername" runat="server" Text="Логин:" AssociatedControlID="TextBoxUsername"></asp:Label><br />
        <asp:TextBox ID="TextBoxUsername" runat="server" Width="300px"></asp:TextBox><br /><br />

        <asp:Label ID="LabelEmail" runat="server" Text="Электронная почта:" AssociatedControlID="TextBoxEmail"></asp:Label><br />
        <asp:TextBox ID="TextBoxEmail" runat="server" Width="300px" TextMode="Email"></asp:TextBox><br /><br />

        <asp:Label ID="LabelPassword" runat="server" Text="Пароль:" AssociatedControlID="TextBoxPassword"></asp:Label><br />
        <asp:TextBox ID="TextBoxPassword" runat="server" Width="300px" TextMode="Password"></asp:TextBox><br /><br />

        <asp:Label ID="LabelConfirmPassword" runat="server" Text="Подтверждение пароля:" AssociatedControlID="TextBoxConfirmPassword"></asp:Label><br />
        <asp:TextBox ID="TextBoxConfirmPassword" runat="server" Width="300px" TextMode="Password"></asp:TextBox>

        <asp:Button ID="ButtonRegister" runat="server" Text="Зарегистрироваться" OnClick="ButtonRegister_Click" />
    </asp:Panel>
</asp:Content>
