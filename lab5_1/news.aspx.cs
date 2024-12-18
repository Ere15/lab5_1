using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace lab5_1
{
    public partial class news : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNews();
            }
        }

        private void LoadNews()
        {
            string searchText = "";

            // Получаем поисковый запрос из сессии, если он есть
            if (Session["SearchText"] != null)
            {
                searchText = Session["SearchText"].ToString();
                TextBoxSearch.Text = searchText; // Отображаем текст в TextBox
            }

            string query = @"SELECT NewsID, Title, Content, PublicationDate FROM News WHERE Title LIKE '%' + @SearchText + '%' OR Content LIKE '%' + @SearchText + '%' ORDER BY PublicationDate DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchText", searchText);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            RepeaterNews.DataSource = dt;
                            RepeaterNews.DataBind();
                            LabelError.Text = "";
                        }
                        else
                        {
                            RepeaterNews.DataSource = null;
                            RepeaterNews.DataBind();
                            LabelError.Text = "Новости не найдены.";
                        }
                    }
                }
            }
        }

        protected void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            // Сохраняем поисковый запрос в сессию
            Session["SearchText"] = TextBoxSearch.Text;
            LoadNews();
        }
    }
}