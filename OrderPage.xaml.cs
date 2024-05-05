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

                try
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
                catch 
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
            //using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            //{
            //    connection.Open();

            //    string query = "INSERT INTO " +
            //        "Insurance_Policy " +
            //        "(" +
            //        "Type_Insurance_Id, " +
            //        "User_Type_Id, " +
            //        "User_Id, " +
            //        "Insurance_Prize, " +
            //        "Insurance_Object, " +
            //        "Insurance_Case, " +
            //        "Insurance_Sum, " +
            //        "Date_Of_Conclusion, " +
            //        "Date_Of_Registration, " +
            //        "Date_Start, " +
            //        "Date_End, " +
            //        "Status, " +
            //        "User_Bank_Id" +
            //        ") " +
            //        "VALUES " +
            //        "(" +
            //        "@Type_Insurance_Id" +
            //        "@User_Type_Id" +
            //        "@User_Id" +
            //        "@Insurance_Prize" +
            //        "@Insurance_Object" +
            //        "@Insurance_Case" +
            //        "@Insurance_Sum" +
            //        "@Date_Of_Conclusion" +
            //        "@Date_Of_Registration" +
            //        "@Date_Start" +
            //        "@Date_End" +
            //        "@Status" +
            //        "@User_Bank_Id" +
            //        ")";
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@Type_Insurance_Id", userBankId);
            //        command.Parameters.AddWithValue("@User_Type_Id", userBankId);
            //        command.Parameters.AddWithValue("@User_Id", userBankId);
            //        command.Parameters.AddWithValue("@Insurance_Prize", userBankId);
            //        command.Parameters.AddWithValue("@Insurance_Object", userBankId);
            //        command.Parameters.AddWithValue("@Insurance_Case", userBankId);
            //        command.Parameters.AddWithValue("@Insurance_Sum", userBankId);
            //        command.Parameters.AddWithValue("@Date_Of_Conclusion", userBankId);
            //        command.Parameters.AddWithValue("@Date_Of_Registration", userBankId);
            //        command.Parameters.AddWithValue("@Date_Start", userBankId);
            //        command.Parameters.AddWithValue("@Date_End", userBankId);
            //        command.Parameters.AddWithValue("@Status", userBankId);
            //        command.Parameters.AddWithValue("@User_Bank_Id", userBankId);

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                UserBankId.Text = reader["Bank"].ToString();
            //                UserPaymentAccount.Text = reader["Payment_Account"].ToString();
            //            }
            //        }
            //    }
            //}

        }
    }
}
