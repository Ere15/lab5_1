<%@ Page Title="Создание курса" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CreateCourse.aspx.cs" Inherits="lab5_1.CreateCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Создание курса</h1>

    <asp:Label ID="ErrorMessageLabel" runat="server" ForeColor="Red"></asp:Label><br />

    <asp:Label ID="CourseNameLabel" runat="server" Text="Название курса:"></asp:Label><br />
    <asp:TextBox ID="CourseNameTextBox" runat="server" CssClass="form-control"></asp:TextBox><br />

    <asp:Panel ID="AssignmentsPanel" runat="server">
        <h2>Задания</h2>
        <asp:Repeater ID="AssignmentsRepeater" runat="server">
            <ItemTemplate>
                <div style="border: 1px solid #ccc; padding: 10px; margin-bottom: 10px;">
                    <h3>Задание #<%# Container.ItemIndex + 1 %></h3>
                    <asp:Label Text="Название задания:" runat="server"></asp:Label><br />
                    <asp:TextBox ID="AssignmentNameTextBox" runat="server" CssClass="form-control" Text='<%# Eval("AssignmentName") %>'></asp:TextBox><br />
                    <asp:Label Text="Описание задания:" runat="server"></asp:Label><br />
                    <asp:TextBox ID="AssignmentDescriptionTextBox" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" Text='<%# Eval("AssignmentDescription") %>'></asp:TextBox><br />
                    <asp:Label Text="Материалы (ссылки через запятую):" runat="server"></asp:Label><br />
                    <asp:TextBox ID="MaterialsTextBox" runat="server" CssClass="form-control" Text='<%# Eval("Materials") %>'></asp:TextBox><br />
                    <asp:HiddenField ID="AssignmentIDHiddenField" runat="server" Value='<%# Eval("AssignmentID") %>' />
                    <asp:Button ID="RemoveAssignmentButton" runat="server" Text="Удалить задание" OnClick="RemoveAssignmentButton_Click" CommandArgument='<%# Container.ItemIndex %>' CssClass="btn btn-danger"/>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Button ID="AddAssignmentButton" runat="server" Text="Добавить задание" OnClick="AddAssignmentButton_Click" CssClass="btn btn-primary"/>
    </asp:Panel>
    <br />
    <asp:Button ID="CreateCourseButton" runat="server" Text="Создать курс" OnClick="CreateCourseButton_Click" CssClass="btn btn-success"/>
</asp:Content>