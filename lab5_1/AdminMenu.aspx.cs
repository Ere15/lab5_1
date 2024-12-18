using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class AdminMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || (int)Session["UserRole"] != 1)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourses();
                ShowContent("EnrollmentRequests");
            }
        }

        protected void AdminMenuControl_MenuItemClick(object sender, MenuEventArgs e)
        {
            ShowContent(e.Item.Value);
        }

        private void ShowContent(string contentType)
        {
            ContentTitleLabel.Text = GetContentTitle(contentType);
            ErrorMessageLabel.Text = "";
            CoursePanel.Visible = contentType == "CourseRegistrations";

            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;
            string query = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    switch (contentType)
                    {
                        case "EnrollmentRequests":
                            query = @"SELECT r.RegistrationID, u.Username, c.CourseName, rs.StatusName, r.UserID, r.CourseID
                                      FROM Registration r
                                      INNER JOIN Users u ON r.UserID = u.UserID
                                      INNER JOIN Courses c ON r.CourseID = c.CourseID
                                      INNER JOIN RegistrationStatus rs ON r.RegistrationStatusID = rs.StatusID";
                            break;
                        case "CourseRequests":
                            query = @"SELECT c.CourseID, c.CourseName, cs.StatusName, c.CourseDescription, ISNULL(u.Username, 'Не назначен') AS TeacherName
                                      FROM Courses c
                                      INNER JOIN CourseStatus cs ON c.CourseStatusID = cs.CourseStatusID
                                      LEFT JOIN Users u ON c.TeacherID = u.UserID";
                            break;
                        case "UsersList":
                            query = "SELECT UserID, Username, Email, RoleID FROM Users";
                            break;
                        case "CourseRegistrations":
                            if (CourseDropDownList.SelectedIndex > 0)
                            {
                                int selectedCourseId = int.Parse(CourseDropDownList.SelectedValue);
                                query = @"SELECT u.Username, u.Email, r.RegistrationID, rs.StatusName
                                          FROM Registration r
                                          INNER JOIN Users u ON r.UserID = u.UserID
                                          INNER JOIN RegistrationStatus rs ON r.RegistrationStatusID = rs.StatusID
                                          WHERE r.CourseID = @CourseID";
                                cmd.Parameters.AddWithValue("@CourseID", selectedCourseId);
                            }
                            else
                            {
                                GridViewData.DataSource = null;
                                GridViewData.DataBind();
                                return;
                            }
                            break;
                    }

                    cmd.CommandText = query;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        GridViewData.DataSource = dt;
                        GridViewData.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessageLabel.Text = "Ошибка при выполнении запроса: " + ex.Message;
                }
            }
        }

        private string GetContentTitle(string contentType)
        {
            switch (contentType)
            {
                case "EnrollmentRequests": return "Заявки на поступление";
                case "CourseRequests": return "Заявки на создание курсов";
                case "UsersList": return "Список пользователей";
                case "CourseRegistrations": return "Записанные на курс";
                default: return "";
            }
        }

        private void LoadCourses()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;
            string query = "SELECT CourseID, CourseName FROM Courses";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    CourseDropDownList.DataSource = dt;
                    CourseDropDownList.DataTextField = "CourseName";
                    CourseDropDownList.DataValueField = "CourseID";
                    CourseDropDownList.DataBind();
                    CourseDropDownList.Items.Insert(0, new ListItem("-- Выберите курс --", "0"));
                }
            }
        }

        protected void CourseDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowContent("CourseRegistrations");
        }

        protected void GridViewData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewData.EditIndex = e.NewEditIndex;
            ShowContent("UsersList");
        }

        protected void GridViewData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewData.EditIndex = -1;
            ShowContent("UsersList");
        }

        protected void GridViewData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;
            int userId = Convert.ToInt32(GridViewData.DataKeys[e.RowIndex].Values["UserID"]); // Используем DataKeyNames
            int newRole = Convert.ToInt32(((DropDownList)GridViewData.Rows[e.RowIndex].FindControl("ddlRole")).SelectedValue);

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UPDATE Users SET RoleID = @Role WHERE UserID = @UserID", conn))
            {
                try
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Role", newRole);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        GridViewData.EditIndex = -1;
                        ShowContent("UsersList");
                    }
                    else
                    {
                        ErrorMessageLabel.Text = "Не удалось обновить роль пользователя.";
                    }

                }
                catch (Exception ex)
                {
                    ErrorMessageLabel.Text = "Ошибка обновления роли: " + ex.Message;
                }
            }
        }
        // ... остальные методы
    }
}