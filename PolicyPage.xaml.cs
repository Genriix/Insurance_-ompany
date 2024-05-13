using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Insurance_сompany
{
    /// <summary>
    /// Логика взаимодействия для PolicyPage.xaml
    /// </summary>
    public partial class PolicyPage : Page
    {
        private string userId = "";
        private string typeInsuranceId = "";
        private string userBankId = "";
        private string typeUserId = "";
        private int statusId = 0;

        public PolicyPage()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                string query = "" +
                    "SELECT " +
                    "Insurance_Policy.*, " +
                    "[User].id AS id " +
                    "FROM Insurance_Policy " +
                    "JOIN [User] " +
                    "ON Insurance_Policy.User_Id = [User].id " +
                    "WHERE Insurance_Policy.id = @PolicyId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PolicyId", Polices.selectedPolicy);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            typeInsuranceId = reader["Type_Insurance_Id"].ToString();
                            userBankId = reader["User_Bank_Id"].ToString();
                            userId = reader["id"].ToString();
                            statusId = Convert.ToInt32(reader["Status"]);
                            

                            Insurance_Prize.Text = reader["Insurance_Prize"].ToString();
                            Insurance_Sum.Text = reader["Insurance_Sum"].ToString();
                            Date_Of_Conclusion.Text = reader["Date_Of_Conclusion"].ToString();
                            Date_Of_Registration.Text = reader["Date_Of_Registration"].ToString();
                            Insurance_Object.Text = reader["Insurance_Object"].ToString();
                            Date_Start.Text = reader["Date_Start"].ToString();
                            Date_End.Text = reader["Date_End"].ToString();
                            Insurance_Case.Text = reader["Insurance_Case"].ToString();
                        }
                    }
                }

                query = "" +
                    "SELECT " +
                    "Type_Insurance.Name " +
                    "FROM Type_Insurance " +
                    "WHERE Type_Insurance.id = @typeInsuranceId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@typeInsuranceId", typeInsuranceId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Type_Insurance_Id.Text = reader["Name"].ToString();
                        }
                    }
                }


                query = "" +
                    "SELECT " +
                    "User_Type_Id " +
                    "FROM [User] " +
                    "WHERE id = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            typeUserId = reader["User_Type_Id"].ToString();
                        }
                    }
                }

                if (typeUserId == "1")
                {
                    User_Type.Text = "Физическое лицо";
                    query = "" +
                        "SELECT " +
                        "F_Name, " +
                        "L_Name, " +
                        "M_Name " +
                        "FROM Individual_User " +
                        "WHERE User_Id = @userId;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FLMNameUser.Text = reader["L_Name"].ToString() + " " + reader["F_Name"].ToString() + " " + reader["M_Name"].ToString();
                            }
                        }
                    }
                }
                else if (typeUserId == "2")
                {
                    User_Type.Text = "Юридическое лицо";
                    query = "" +
                        "SELECT " +
                        "Name " +
                        "FROM Legal_User " +
                        "WHERE User_Id = @userId;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FLMNameUser.Text = reader["Name"].ToString();
                            }
                        }
                    }
                }

                query = "" +
                    "SELECT " +
                    "Bank, " +
                    "Payment_Account " +
                    "FROM UserBank " +
                    "WHERE id = @userBankId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userBankId", userBankId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserBankId.Text = reader["Bank"].ToString();
                            UserPaymentAccount.Text = reader["Payment_Account"].ToString();
                        }
                    }
                }
                query = "SELECT Status FROM Statuses";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Statuses.Items.Add(reader["Status"].ToString());
                        }
                    }
                }

                Statuses.SelectedIndex = statusId - 1;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            


            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                if (MessageBox.Show("Вы уверены, что хотите изменить даннные?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    connection.Open();
                    string query = "" +
                        "UPDATE Insurance_Policy SET " +
                        "Status = @statusId " +
                        "WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@statusId", Statuses.SelectedIndex + 1);
                        command.Parameters.AddWithValue("@id", Polices.selectedPolicy);

                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные были обновлены!", "Успех!");
                }
            }
        }
    }
}
