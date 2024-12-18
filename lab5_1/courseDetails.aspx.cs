using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace lab5_1
{
    public partial class courseDetails : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CourseID"] != null)
                {
                    int courseID = int.Parse(Request.QueryString["CourseID"]);
                    LoadCourseDetails(courseID);
                }
                else
                {
                    LabelError.Text = "Некорректный ID курса.";
                }
            }
        }

        private void LoadCourseDetails(int courseID)
        {
            string query = @"
                SELECT c.CourseName, c.CourseDescription, cat.CategoryName, u.Username AS TeacherUsername, cs.StatusName AS CourseStatus -- Добавлено
                FROM Courses c
                JOIN Category cat ON c.CategoryID = cat.CategoryID
                JOIN Users u ON c.TeacherID = u.UserID
                LEFT JOIN CourseStatus cs ON c.CourseStatusID = cs.CourseStatusID -- LEFT JOIN
                WHERE c.CourseID = @CourseID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("CourseID",SqlDbType.Int).Value = courseID;

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                LabelCourseStatusValue.Text = reader["CourseStatus"] == DBNull.Value ? "" : reader["CourseStatus"].ToString();
                                LabelCourseName.Text = reader["CourseName"].ToString();
                                LabelCourseDescription.Text = reader["CourseDescription"].ToString();
                                LabelCategoryValue.Text = reader["CategoryName"].ToString();
                                LabelTeacherValue.Text = reader["TeacherUsername"].ToString();
                            }
                            else
                            {
                                LabelError.Text = "Курс не найден.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LabelError.Text = "Ошибка при загрузке данных курса: " + ex.Message;
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}