using Microsoft.Office.Core;
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


            if (InsurancePolicies.SelectedItem != null)
            {
                DataRowView row = (DataRowView)InsurancePolicies.SelectedItem;
                selectedPolicy = row["id"].ToString();
            }



            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                Word.Application application = new Word.Application();
                Document document = application.Documents.Add();
                document.Paragraphs.SpaceAfter = 0;
                document.Paragraphs.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;

                Word.Paragraph paragraph = document.Paragraphs.Add();
                Range range = paragraph.Range;
                range.Font.Name = "Times New Roman";
                range.Font.Bold = 1;
                range.Font.Size = 26;
                range.Text = $"Страховой полис";
                range.InsertParagraphAfter();
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                Word.Paragraph paragraph0 = document.Paragraphs.Add();
                Range range0 = paragraph0.Range;
                range0.Font.Name = "Times New Roman";
                range0.Font.Bold = 0;
                range0.Font.Size = 14;

                string query = "" +
                    "SELECT " +
                    "User_Type.UserTypeName, " +
                    "Insurance_Policy.User_Type_Id, " +
                    "Type_Insurance.Name, " +
                    "Insurance_Policy.Type_Insurance_Id " +
                    "FROM " +
                    "Insurance_Policy " +
                    "JOIN " +
                    "User_Type " +
                    "ON " +
                    "User_Type.id = Insurance_Policy.User_Type_Id " +
                    "JOIN " +
                    "Type_Insurance " +
                    "ON " +
                    "Type_Insurance.id = Insurance_Policy.Type_Insurance_Id " +
                    "WHERE " +
                    "Insurance_Policy.id = @selectedPolicy;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectedPolicy", selectedPolicy);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            range0.Text = $"" +
                                $"Тип страхователя: {reader["UserTypeName"]}.\n" +
                                $"Тип страхового договора: {reader["Name"]}";
                        }
                    }
                }
                range0.InsertParagraphAfter();
                range0.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;

                application.Visible = true;
            }
        }

        private void ShowUserFullInfo_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new FullIndividualUserInfo());
        }
    }
}
