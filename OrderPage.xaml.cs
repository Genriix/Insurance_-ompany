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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        private string userId = "";
        private string typeInsuranceId = "";
        private string userBankId = "";
        private string typeUserId = "";
        private DateTime Date_Of_Registration;

        public OrderPage()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                string query = "" +
                    "SELECT " +
                    "[Order].Type_Insurance_Id, " +
                    "[Order].Insurance_Object, " +
                    "[Order].User_Bank_Id, " +
                    "[Order].Filing_Date, " +
                    "[User].id " +
                    "FROM [Order] " +
                    "JOIN [User] " +
                    "ON [Order].User_Id = [User].id " +
                    "WHERE [Order].id = @OrderId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", ManagerPage.selectedOrder);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            typeInsuranceId = reader["Type_Insurance_Id"].ToString();
                            Insurance_Object.Text = reader["Insurance_Object"].ToString();
                            userBankId = reader["User_Bank_Id"].ToString();
                            Date_Of_Registration = Convert.ToDateTime(reader["Filing_Date"]);
                            userId = reader["id"].ToString();
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
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DateTime dateStart = Date_Start.SelectedDate.GetValueOrDefault();
            int insurance_Id = 0;


            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                string query = "INSERT INTO " +
                    "Insurance_Policy " +
                    "(" +
                    "Type_Insurance_Id, " +
                    "User_Type_Id, " +
                    "User_Id, " +
                    "Insurance_Prize, " +
                    "Insurance_Object, " +
                    "Insurance_Case, " +
                    "Insurance_Sum, " +
                    "Date_Of_Conclusion, " +
                    "Date_Of_Registration, " +
                    "Date_Start, " +
                    "Date_End, " +
                    "Status, " +
                    "User_Bank_Id, " +
                    "Manager_Id" +
                    ") " +
                    "VALUES " +
                    "(" +
                    "@Type_Insurance_Id, " +
                    "@User_Type_Id, " +
                    "@User_Id, " +
                    "@Insurance_Prize, " +
                    "@Insurance_Object, " +
                    "@Insurance_Case, " +
                    "@Insurance_Sum, " +
                    "@Date_Of_Conclusion, " +
                    "@Date_Of_Registration, " +
                    "@Date_Start, " +
                    "@Date_End, " +
                    "@Status, " +
                    "@User_Bank_Id, " +
                    "@Manager_Id" +
                    "); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Type_Insurance_Id", typeInsuranceId);
                    command.Parameters.AddWithValue("@User_Type_Id", typeUserId);
                    command.Parameters.AddWithValue("@User_Id", userId);
                    command.Parameters.AddWithValue("@Insurance_Prize", Insurance_Prize.Text);
                    command.Parameters.AddWithValue("@Insurance_Object", Insurance_Object.Text);
                    command.Parameters.AddWithValue("@Insurance_Case", Insurance_Case.Text);
                    command.Parameters.AddWithValue("@Insurance_Sum", Insurance_Sum.Text);
                    command.Parameters.AddWithValue("@Date_Of_Conclusion", DateTime.Now);
                    command.Parameters.AddWithValue("@Date_Of_Registration", Date_Of_Registration);
                    command.Parameters.AddWithValue("@Date_Start", dateStart);
                    command.Parameters.AddWithValue("@Date_End", dateStart.AddMonths(int.Parse(Date_Of_Conclusion.Text)));
                    command.Parameters.AddWithValue("@Status", 1);
                    command.Parameters.AddWithValue("@User_Bank_Id", userBankId);
                    command.Parameters.AddWithValue("@Manager_Id", LoginPage.manager_id);

                    insurance_Id = Convert.ToInt32(command.ExecuteScalar());
                }

                query = "DELETE FROM [Order] WHERE id = @orderId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", ManagerPage.selectedOrder);

                    command.ExecuteNonQuery();
                }
                query = "INSERT INTO Reports (Manager_Id, Date_Operation, Id_Opeation, Id_Policy) VALUES (@Manager_Id, @Date_Operation, 1, @Id_Policy)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Manager_Id", LoginPage.manager_id);
                    command.Parameters.AddWithValue("@Date_Operation", DateTime.Now);
                    command.Parameters.AddWithValue("@Id_Policy", insurance_Id);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Страховой полис офрмлен", "Успех!");
                Manager.MainFrame.GoBack();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                string query = query = "DELETE FROM [Order] WHERE id = @orderId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", ManagerPage.selectedOrder);

                    command.ExecuteNonQuery();
                }
                query = "INSERT INTO Reports (Manager_Id, Date_Operation, Id_Opeation) VALUES (@Manager_Id, @Date_Operation, 2)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Manager_Id", LoginPage.manager_id);
                    command.Parameters.AddWithValue("@Date_Operation", DateTime.Now);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Заявка отклонена", "Успех!");
                Manager.MainFrame.GoBack();
            }
        }
    }
}
