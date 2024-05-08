using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
using Word = Microsoft.Office.Interop.Word;

namespace Insurance_сompany
{
    /// <summary>
    /// Логика взаимодействия для UserInfoPage.xaml
    /// </summary>
    public partial class UserInfoPage : System.Windows.Controls.Page
    {
        private int insuranceTypeID;
        private static string selectedPolicy;


        public UserInfoPage()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();
                string query = "SELECT Type_Insurance.Name, Insurance_Policy.id, Insurance_Policy.Date_Of_Conclusion, Insurance_Policy.Insurance_Object, Insurance_Policy.Status FROM Insurance_Policy JOIN Type_Insurance ON Insurance_Policy.Type_Insurance_Id = Type_Insurance.id WHERE User_Id = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", LoginPage.user_id);

                    using (System.Data.DataTable dataTable = new System.Data.DataTable())
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            try
                            {
                                adapter.Fill(dataTable);

                                InsurancePolicies.ItemsSource = dataTable.DefaultView;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                }
                query = "SELECT Type_Insurance.Name, [Order].Insurance_Object, [Order].Filing_Date FROM [Order] JOIN Type_Insurance ON [Order].Type_Insurance_Id = Type_Insurance.id WHERE User_Id = @userId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", LoginPage.user_id);
                    using (System.Data.DataTable dataTable = new System.Data.DataTable())
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            try
                            {
                                adapter.Fill(dataTable);

                                Orders.ItemsSource = dataTable.DefaultView;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                }

                query = "SELECT Telephone_Number FROM [User] WHERE id = @UserId;";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", LoginPage.user_id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            T_Number.Text = reader["Telephone_Number"].ToString();
                        }
                    }
                }

                query = "SELECT F_Name, L_Name, M_Name FROM Individual_User WHERE User_Id = @UserId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", LoginPage.user_id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            F_Name.Text = reader["F_Name"].ToString();
                            L_Name.Text = reader["L_Name"].ToString();
                            M_Name.Text = reader["M_Name"].ToString();
                        }
                    }
                }
            }
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            var application = new Word.Application();
            Word.Document document = application.Documents.Add();


            if (InsurancePolicies.SelectedItem != null)
            {
                DataRowView row = (DataRowView)InsurancePolicies.SelectedItem;
                selectedPolicy = row["id"].ToString();
                Console.WriteLine("id выбранного договора: " + selectedPolicy);
            }



            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                string query = "SELECT Type_Insurance_Id FROM Insurance_Policy WHERE id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", selectedPolicy);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            insuranceTypeID = reader.GetInt32(0);
                            Console.WriteLine("id выбранного типа договора: " + insuranceTypeID);
                        }
                    }
                }

                query = "SELECT Name FROM Type_Insurance WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", insuranceTypeID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Word.Paragraph paragraph = document.Paragraphs.Add();
                        Word.Range range = paragraph.Range;
                        if (reader.Read())
                        {
                            range.Text = $"Страховой полис. Тип страхового договора: {reader["Name"]}";
                        }
                    }
                }


                //query = "select * from insurance_policy where user_id = @user_id";
                //using (SqlCommand command = new SqlCommand(query, connection))
                //{
                //    command.Parameters.AddWithValue("@user_id", LoginPage.user_id);

                //    Word.Paragraph paragraph = document.Paragraphs.Add();
                //    Word.Range range = paragraph.Range;
                //    range.Text = "Барабарабара береберебере";
                //}
            }
            application.Visible = true;
        }

        private void ShowUserFullInfo_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new FullIndividualUserInfo());
        }
    }
}
