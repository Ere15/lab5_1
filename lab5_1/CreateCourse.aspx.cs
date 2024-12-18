using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls; // Добавляем using System.Linq

namespace lab5_1
{
    public partial class CreateCourse : System.Web.UI.Page
    {
        [Serializable]
        public class AssignmentData
        {
            public Guid Id { get; set; }
            public string AssignmentName { get; set; }
            public string AssignmentDescription { get; set; }
            public string Materials { get; set; }

            public AssignmentData()
            {
                Id = Guid.NewGuid();
            }
        }

        private List<AssignmentData> assignments;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || (int)Session["UserRole"] != 2)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Session["Assignments"] = new List<AssignmentData>();
            }
            assignments = (List<AssignmentData>)Session["Assignments"];
            BindAssignments();
        }

        protected void AddAssignmentButton_Click(object sender, EventArgs e)
        {
            assignments.Add(new AssignmentData());
            Session["Assignments"] = assignments;
            BindAssignments();
        }

        protected void RemoveAssignmentButton_Click(object sender, EventArgs e)
        {
            Guid idToRemove = Guid.Parse(((Button)sender).CommandArgument);
            assignments.RemoveAll(a => a.Id == idToRemove); // Используем RemoveAll и лямбда-выражение
            Session["Assignments"] = assignments;
            BindAssignments();
        }


        private void BindAssignments()
        {
            AssignmentsRepeater.DataSource = assignments;
            AssignmentsRepeater.DataBind();
        }

        protected void CreateCourseButton_Click(object sender, EventArgs e)
        {
            string courseName = CourseNameTextBox.Text;
            int teacherId;

            if (Session["UserID"] != null)
            {
                if (!int.TryParse(Session["UserID"].ToString(), out teacherId))
                {
                    ErrorMessageLabel.Text = "Ошибка: некорректный ID пользователя в сессии.";
                    return;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Courses (CourseName, TeacherID, CourseStatusID) VALUES (@CourseName, @TeacherID, 1); SELECT SCOPE_IDENTITY();", conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseName", courseName);
                        cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                        int courseId = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (var assignmentData in assignments)
                        {
                            using (SqlCommand assignmentCmd = new SqlCommand("INSERT INTO Assignments (AssignmentName, AssignmentDescription, TopicID, Deadline, AssignmentStatusID) VALUES (@AssignmentName, @AssignmentDescription, @CourseId, GETDATE(), 1); SELECT SCOPE_IDENTITY();", conn))
                            {
                                assignmentCmd.Parameters.AddWithValue("@AssignmentName", assignmentData.AssignmentName ?? "");
                                assignmentCmd.Parameters.AddWithValue("@AssignmentDescription", assignmentData.AssignmentDescription ?? "");
                                assignmentCmd.Parameters.AddWithValue("@CourseId", courseId);
                                int assignmentId = Convert.ToInt32(assignmentCmd.ExecuteScalar());

                                string[] materials = assignmentData.Materials?.Split(',');
                                if (materials != null)
                                {
                                    foreach (string material in materials)
                                    {
                                        using (SqlCommand materialCmd = new SqlCommand("INSERT INTO Materials (MaterialName, ResourceLink, AssignmentID) VALUES (@MaterialName, @ResourceLink, @AssignmentId)", conn))
                                        {
                                            materialCmd.Parameters.AddWithValue("@MaterialName", material.Trim());
                                            materialCmd.Parameters.AddWithValue("@ResourceLink", material.Trim());
                                            materialCmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                                            materialCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Session.Remove("Assignments");
                    Response.Redirect("TeacherProfile.aspx");
                }
                catch (Exception ex)
                {
                    ErrorMessageLabel.Text = "Ошибка создания курса: " + ex.Message;
                }
            }
        }
    }
}