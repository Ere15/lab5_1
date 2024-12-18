<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="course.aspx.cs" Inherits="lab5_1.course" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Курсы</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Курсы</h1>

    <div style="display: flex; gap: 20px; margin-bottom: 20px;">
        <div>
            <asp:Label ID="LabelCategory" runat="server" Text="Категория:"></asp:Label><br />
            <asp:DropDownList ID="DropDownListCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListCategory_SelectedIndexChanged">
                <asp:ListItem Value="0">Все</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            <asp:Label ID="LabelTeacher" runat="server" Text="Преподаватель:"></asp:Label><br />
            <asp:DropDownList ID="DropDownListTeacher" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListTeacher_SelectedIndexChanged">
                <asp:ListItem Value="0">Все</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            <asp:Label ID="LabelSearch" runat="server" Text="Поиск по названию:"></asp:Label><br />
            <asp:TextBox ID="TextBoxSearch" runat="server" AutoPostBack="true" OnTextChanged="TextBoxSearch_TextChanged"></asp:TextBox>
        </div>
    </div>

    <asp:GridView ID="GridViewCourses" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnSelectedIndexChanged="GridViewCourses_SelectedIndexChanged">
        <Columns>
            <asp:HyperLinkField DataTextField="CourseName" DataNavigateUrlFormatString="courseDetails.aspx?CourseID={0}" DataNavigateUrlFields="CourseID" HeaderText="Название курса" />
            <asp:BoundField DataField="CourseDescription" HeaderText="Описание" />
            <asp:BoundField DataField="CategoryName" HeaderText="Категория" />
            <asp:BoundField DataField="TeacherUsername" HeaderText="Преподаватель" />
            <asp:BoundField DataField="CourseStatus" HeaderText="Статус" />
        </Columns>
    </asp:GridView>
</asp:Content>