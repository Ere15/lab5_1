using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class reg : System.Web.UI.Page
    {
        protected void ButtonRegister_Click(object sender, EventArgs e)
        {
            // Получение данных из полей
            string username = TextBoxUsername.Text.Trim();
            string email = TextBoxEmail.Text.Trim();
            string password = TextBoxPassword.Text;
            string confirmPassword = TextBoxConfirmPassword.Text;

            // Проверка совпадения паролей
            if (password != confirmPassword)
            {
                DisplayMessage("Пароли не совпадают!", false);
                return;
            }

            // Проверка заполненности полей
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                DisplayMessage("Пожалуйста, заполните все поля!", false);
                return;
            }

            // Получение строки подключения
            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // SQL-запрос для добавления нового пользователя (БЕЗ RoleID)
                    string query = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Передача параметров в запрос
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password); // В реальном проекте нужно использовать хэширование пароля!
                        cmd.Parameters.AddWithValue("@Email", email);

                        // Открытие соединения и выполнение команды
                        conn.Open();
                        int rowsInserted = cmd.ExecuteNonQuery();

                        if (rowsInserted > 0)
                        {
                            DisplayMessage("Регистрация успешна!", true);
                        }
                        else
                        {
                            DisplayMessage("Ошибка при регистрации.", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                DisplayMessage("Произошла ошибка: " + ex.Message, false);
            }

            Response.Redirect("Default.aspx");
        }

        // Метод для отображения сообщений
        private void DisplayMessage(string message, bool isSuccess)
        {
            var messageColor = isSuccess ? "Green" : "Red";
            RegistrationPanel.Controls.Add(new Literal
            {
                Text = $"<p style='color:{messageColor};font-weight:bold;'>{message}</p>"
            });
        }
    }
}