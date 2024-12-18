using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class CreateCourse : System.Web.UI.Page
    {
        private List<AssignmentData> assignments = new List<AssignmentData>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || (int)Session["UserRole"] != 2)
            {
                Response.Redirect("Default.aspx");
                return;
            }
            if (!IsPostBack)
            {
                ViewState["Assignments"] = assignments;
            }
            else
            {
                assignments = (List<AssignmentData>)ViewState["Assignments"];
            }
            BindAssignments();
        }

        protected void AddAssignmentButton_Click(object sender, EventArgs e)
        {
            assignments.Add(new AssignmentData());
            ViewState["Assignments"] = assignments;
            BindAssignments();
        }

        protected void RemoveAssignmentButton_Click(object sender, EventArgs e)
        {
            int index = int.Parse(((Button)sender).CommandArgument);
            assignments.RemoveAt(index);
            ViewState["Assignments"] = assignments;
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
            int teacherId = (int)Session["UserID"]; // Получаем ID преподавателя из сессии
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
                            using (SqlCommand assignmentCmd = new SqlCommand("INSERT INTO Assignments (AssignmentName, AssignmentDescription, TopicID, Deadline, AssignmentStatusID) VALUES (@AssignmentName, @AssignmentDescription, NULL, GETDATE(), 1); SELECT SCOPE_IDENTITY();", conn))
                            {
                                assignmentCmd.Parameters.AddWithValue("@AssignmentName", assignmentData.AssignmentName);
                                assignmentCmd.Parameters.AddWithValue("@AssignmentDescription", assignmentData.AssignmentDescription);
                                int assignmentId = Convert.ToInt32(assignmentCmd.ExecuteScalar());

                                string[] materials = assignmentData.Materials?.Split(',');
                                if (materials != null)
                                {
                                    foreach (string material in materials)
                                    {
                                        using (SqlCommand materialCmd = new SqlCommand("INSERT INTO Materials (MaterialName, ResourceLink) VALUES (@MaterialName, @ResourceLink)", conn))
                                        {
                                            materialCmd.Parameters.AddWithValue("@MaterialName", material.Trim());
                                            materialCmd.Parameters.AddWithValue("@ResourceLink", material.Trim());
                                            materialCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Response.Redirect("TeacherProfile.aspx");
                }
                catch (Exception ex)
                {
                    ErrorMessageLabel.Text = "Ошибка создания курса: " + ex.Message;
                }
            }
        }

        public class AssignmentData
        {
            public int AssignmentID { get; set; }
            public string AssignmentName { get; set; }
            public string AssignmentDescription { get; set; }
            public string Materials { get; set; }
        }
    }
}