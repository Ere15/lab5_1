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
                    string query = "SELECT RoleID, UserID FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();
                        object roleResult = cmd.ExecuteScalar();

                        if (roleResult != null)
                        {
                            // Успешная авторизация
                            int userRole = Convert.ToInt32(roleResult);
                            Session["Username"] = username; // Сохраняем имя пользователя
                            Session["UserRole"] = userRole;
                            Session["UserID"] = roleResult.ToString();
                            if (userRole == 1)
                            {
                                Response.Redirect("AdminMenu.aspx");
                            }
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

        protected void GoToProfile_Click(object sender, EventArgs e)
        {
            if (Session["UserRole"] != null) // Проверяем, авторизован ли пользователь
            {
                int userRole = (int)Session["UserRole"];

                switch (userRole)
                {
                    case 1:
                        Response.Redirect("AdminMenu.aspx");
                        break;
                    case 2:
                        Response.Redirect("TeacherProfile.aspx");
                        break;
                    case 3:
                        Response.Redirect("personal.aspx");
                        break;
                    default:
                        // Обработка неизвестной роли (опционально, можно просто ничего не делать)
                        break;
                }
            }
            else
            {
                // Пользователь не авторизован, можно перенаправить на страницу входа или показать сообщение
                Response.Redirect("Default.aspx"); // Перенаправление на страницу входа
                                                   // Или:
                                                   // Label1.Text = "Пожалуйста, войдите в систему.";
            }
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