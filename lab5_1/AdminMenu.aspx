<%@ Page Title="Admin Menu" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminMenu.aspx.cs" Inherits="lab5_1.AdminMenu" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Админ-меню</h1>

    <asp:Menu ID="AdminMenuControl" runat="server" Orientation="Vertical" OnMenuItemClick="AdminMenuControl_MenuItemClick">
        <Items>
            <asp:MenuItem Text="Заявки на поступление" Value="EnrollmentRequests" />
            <asp:MenuItem Text="Заявки на создание курсов" Value="CourseRequests" />
            <asp:MenuItem Text="Список пользователей" Value="UsersList" />
            <asp:MenuItem Text="Записанные на курс" Value="CourseRegistrations" />
        </Items>
    </asp:Menu>

    <asp:Panel ID="ContentPanel" runat="server">
        <h2><asp:Label ID="ContentTitleLabel" runat="server"></asp:Label></h2>
        <asp:Panel ID="CoursePanel" runat="server" Visible="false">
            <asp:DropDownList ID="CourseDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CourseDropDownList_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
        </asp:Panel>
        <asp:GridView ID="GridViewData" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped"
            OnRowEditing="GridViewData_RowEditing" OnRowCancelingEdit="GridViewData_RowCancelingEdit"
            OnRowUpdating="GridViewData_RowUpdating" DataKeyNames="UserID">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="Username" HeaderText="Имя дауна" ReadOnly="true" />
                <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
                <asp:TemplateField HeaderText="Роль">
                    <ItemTemplate>
                        <asp:Label ID="lblRole" runat="server" Text='<%# Eval("RoleID") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" SelectedValue='<%# Bind("RoleID") %>'>
                            <asp:ListItem Value="1">Администратор</asp:ListItem>
                            <asp:ListItem Value="2">Преподаватель</asp:ListItem>
                            <asp:ListItem Value="3">Студент</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" EditText="Изменить" CancelText="Отмена" UpdateText="Сохранить" />
            </Columns>
        </asp:GridView>
        <asp:Label ID="ErrorMessageLabel" runat="server" ForeColor="Red"></asp:Label>
    </asp:Panel>
</asp:Content>