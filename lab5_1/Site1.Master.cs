using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateUI();
            }
        }

        protected void Button1_Click(object sender, EventArgs e) // Обработчик кнопки "Вход"
        {
            string username = TextBox1.Text.Trim();
            string password = TextBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Label1.Text = "Введите логин и пароль.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();
                        int userExists = (int)cmd.ExecuteScalar();

                        if (userExists > 0)
                        {
                            // Успешная авторизация
                            Session["Username"] = username; // Сохраняем имя пользователя
                            Label1.Text = $"Добро пожаловать, {username}!";
                            UpdateUI();
                        }
                        else
                        {
                            Label1.Text = "Неверный логин или пароль.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Label1.Text = "Ошибка подключения: " + ex.Message;
            }
        }

        protected void Button2_Click(object sender, EventArgs e) // Обработчик кнопки "Регистрация"
        {
            Response.Redirect("~/reg.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e) // Обработчик кнопки "Выход"
        {
            Session.Clear();
            UpdateUI();
            Response.Redirect("Default.aspx");
            Label1.Text = "Вы вышли из системы.";
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("course.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("news.aspx");
        }

        protected void GoToProfile_Click(object sender, EventArgs e) // Переход в личный кабинет
        {
            Response.Redirect("~/personal.aspx");
        }

        private void UpdateUI()
        {
            bool isLoggedIn = Session["Username"] != null;

            // Управление видимостью элементов
            loginFields.Visible = !isLoggedIn;              // Поля ввода логина и пароля
            authenticatedButtons.Style["display"] = isLoggedIn ? "block" : "none"; // Кнопки "Личный кабинет" и "Выход"
        }
    }
}