<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="courseDetails.aspx.cs" Inherits="lab5_1.courseDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Детали курса</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Детали курса</h1>

    <asp:Label ID="LabelCourseName" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label><br />
    <asp:Label ID="LabelCourseDescription" runat="server"></asp:Label><br /><br />
    <asp:Label ID="LabelCategory" runat="server" Font-Bold="true">Категория: </asp:Label><asp:Label ID="LabelCategoryValue" runat="server"></asp:Label><br />
    <asp:Label ID="LabelTeacher" runat="server" Font-Bold="true">Преподаватель: </asp:Label><asp:Label ID="LabelTeacherValue" runat="server"></asp:Label><br />
    <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <asp:Label ID="LabelCourseStatusValue" runat="server"></asp:Label>
</asp:Content>