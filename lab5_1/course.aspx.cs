using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class course : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadTeachers();
                LoadCourses();
            }
        }

        private void LoadCategories()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT CategoryID, CategoryName FROM Category", connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DropDownListCategory.DataSource = dt;
                    DropDownListCategory.DataTextField = "CategoryName";
                    DropDownListCategory.DataValueField = "CategoryID";
                    DropDownListCategory.DataBind();
                    DropDownListCategory.Items.Insert(0, new ListItem("Все", "0"));
                }
            }
        }

        private void LoadTeachers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT UserID, Username FROM Users WHERE RoleID = 2", connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DropDownListTeacher.DataSource = dt;
                    DropDownListTeacher.DataTextField = "Username";
                    DropDownListTeacher.DataValueField = "UserID";
                    DropDownListTeacher.DataBind();
                    DropDownListTeacher.Items.Insert(0, new ListItem("Все", "0"));
                }
            }
        }

        private void LoadCourses()
        {
            string query = @"
                SELECT c.CourseID, c.CourseName, c.CourseDescription, cat.CategoryName, u.Username AS TeacherUsername, cs.StatusName AS CourseStatus -- Добавлено
                FROM Courses c
                JOIN Category cat ON c.CategoryID = cat.CategoryID
                JOIN Users u ON c.TeacherID = u.UserID
                LEFT JOIN CourseStatus cs ON c.CourseStatusID = cs.CourseStatusID -- LEFT JOIN, чтобы отображать все курсы
                WHERE (@CategoryID = 0 OR c.CategoryID = @CategoryID)
                  AND (@TeacherID = 0 OR c.TeacherID = @TeacherID)
                  AND (c.CourseName LIKE '%' + @SearchText + '%' OR c.CourseDescription LIKE '%' + @SearchText + '%')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // ***ОТКРЫВАЕМ СОЕДИНЕНИЕ ЗДЕСЬ!!!***

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryID", DropDownListCategory.SelectedValue);
                        command.Parameters.AddWithValue("@TeacherID", DropDownListTeacher.SelectedValue);
                        command.Parameters.AddWithValue("@SearchText", TextBoxSearch.Text);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            GridViewCourses.DataSource = dt;
                            GridViewCourses.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

            }
        }

        protected void DropDownListCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourses();
        }

        protected void DropDownListTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourses();
        }

        protected void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCourses();
        }

        protected void GridViewCourses_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}