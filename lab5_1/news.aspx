<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="news.aspx.cs" Inherits="lab5_1.news" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Новости</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Новости</h1>

    <div style="margin-bottom: 20px;">
        <asp:Label ID="LabelSearch" runat="server" Text="Поиск:"></asp:Label>
        <asp:TextBox ID="TextBoxSearch" runat="server" AutoPostBack="true" OnTextChanged="TextBoxSearch_TextChanged"></asp:TextBox>
    </div>

    <asp:Repeater ID="RepeaterNews" runat="server">
        <ItemTemplate>
            <div style="border: 1px solid #ccc; padding: 10px; margin-bottom: 10px;">
                <h3><%# Eval("Title") %></h3>
                <p><%# Eval("Content") %></p>
                <p style="font-size: smaller;">Опубликовано: <%# Eval("PublicationDate", "{0:dd.MM.yyyy HH:mm}") %></p>
            </div>
        </ItemTemplate>
        <SeparatorTemplate>
            <hr />
        </SeparatorTemplate>
    </asp:Repeater>

    <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>