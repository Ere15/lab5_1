using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace lab5_1
{
    public partial class personal : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Проверяем, что пользователь авторизован
                if (Session["Username"] != null)
                {
                    string username = Session["Username"].ToString();
                    LoadUserData(username);
                }
                else
                {
                    // Если не авторизован, редиректим на страницу входа
                    Response.Redirect("~/login.aspx");
                }
            }
        }

        // Метод для загрузки данных пользователя
        private void LoadUserData(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;
            string query = "SELECT Username, Email FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    TextBoxUsername.Text = reader["Username"].ToString();
                    TextBoxEmail.Text = reader["Email"].ToString();
                }

                reader.Close();
            }
        }

        // Обработчик для кнопки "Сохранить изменения"
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string username = TextBoxUsername.Text;
            string newEmail = TextBoxEmail.Text;

            // Проверяем, что электронная почта валидна
            if (IsValidEmail(newEmail))
            {
                UpdateUserEmail(username, newEmail);
            }
            else
            {
                // Выводим ошибку
                // Например, можно использовать Label для ошибок
                // LabelError.Text = "Неверный формат электронной почты.";
            }
        }

        // Метод для обновления почты в базе данных
        private void UpdateUserEmail(string username, string newEmail)
        {
            // Проверка на правильность формата электронной почты
            if (string.IsNullOrEmpty(newEmail) || !newEmail.Contains("@"))
            {
                // Выводим ошибку, если почта некорректная
                LabelError.Text = "Неверный формат электронной почты.";
                LabelSuccess.Visible = false;
                LabelError.Visible = true;
                return; // Прерываем выполнение, если почта неверная
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MusicSchoolDB"].ConnectionString;
            string query = "UPDATE Users SET Email = @Email WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", newEmail);
                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Если данные обновлены, показываем успешное сообщение
                        LabelSuccess.Text = "Данные успешно обновлены!";
                        LabelSuccess.Visible = true;
                        LabelError.Visible = false; // Скрываем ошибки, если операция успешна
                    }
                    else
                    {
                        // Если не удалось обновить, показываем ошибку
                        LabelError.Text = "Ошибка при обновлении данных.";
                        LabelError.Visible = true;
                        LabelSuccess.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    // Ловим любые исключения и выводим их
                    LabelError.Text = "Произошла ошибка при обновлении данных: " + ex.Message;
                    LabelError.Visible = true;
                    LabelSuccess.Visible = false;
                }
            }
        }


        // Метод для проверки формата электронной почты
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
