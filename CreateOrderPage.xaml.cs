using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace Insurance_сompany
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderPage.xaml
    /// </summary>
    public partial class CreateOrderPage : Page
    {
        public CreateOrderPage()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();
                string query = "SELECT Name FROM Type_Insurance";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            try
                            {
                                adapter.Fill(dataTable);

                                TypeInsurance.ItemsSource = dataTable.DefaultView;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                }

                query = "SELECT DISTINCT  Bank, Payment_Account FROM UserBank WHERE User_Id = @user_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", LoginPage.user_id); 
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable(); 
                        adapter.Fill(dataTable);
                        IEnumerable<string> uniqueBanks = dataTable.AsEnumerable()
                                .Select(row => row.Field<string>("Bank"))
                                .Distinct();

                        Bank.ItemsSource = uniqueBanks.ToList();
                        Account.ItemsSource = dataTable.AsEnumerable().Select(row => row.Field<string>("Payment_Account")).ToList();
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();

                int user_type_id = 0;
                int selectedIndex = TypeInsurance.SelectedIndex;

                string query = "SELECT User_Type_Id FROM [User] WHERE id = @user_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", LoginPage.user_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user_type_id = reader.GetInt32(0);
                        }
                    }
                }

                if (user_type_id == 1)
                {
                    query = "SELECT Passport_Num, Burth_Date FROM Individual_User WHERE User_Id = @user_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", LoginPage.user_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                reader.Read();
                                if ((reader["Passport_Num"].ToString() == "" && reader["Burth_Date"].ToString() == "") || reader["Passport_Num"].ToString() == "" || reader["Burth_Date"].ToString() == "")
                                {
                                    MessageBox.Show("Введите Номер паспорта и дату рождения в личном кабиненте", "Ошибка!");
                                    return;
                                }
                            }
                            catch { }
                        }
                    }
                } // Паспорт

                if ((Bank.ToString() == null && Account.ToString() == null) || Bank.ToString() == null || Account.ToString() == null)
                {
                    MessageBox.Show("Введите название банка и номер рассчётного счёта", "Ошибка!");
                    return;
                }
                int User_Bank_Id = 0;

                query = "SELECT id FROM UserBank WHERE Bank = @bank AND Payment_Account = @paymentAccount AND User_Id = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bank", Bank.Text);
                    command.Parameters.AddWithValue("@paymentAccount", Account.Text);
                    command.Parameters.AddWithValue("@userId", LoginPage.user_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User_Bank_Id = reader.GetInt32(0);
                            Console.WriteLine($"Банк найден, Банку присвоено значение {User_Bank_Id}");
                        }
                    }
                }
                if (User_Bank_Id == 0) 
                { 
                    query = "INSERT INTO UserBank (Bank, Payment_Account, User_Id) VALUES (@bank, @paymentAccount, @user_id)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bank", Bank.Text);
                        command.Parameters.AddWithValue("@paymentAccount", Account.Text);
                        command.Parameters.AddWithValue("@user_id", LoginPage.user_id);

                        command.ExecuteNonQuery();
                        Console.WriteLine($"Банк не найден, новый банк добавлен");
                    }

                    query = "SELECT id FROM UserBank WHERE Bank = @bank AND Payment_Account = @paymentAccount AND User_Id = @userId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bank", Bank.Text);
                        command.Parameters.AddWithValue("@paymentAccount", Account.Text);
                        command.Parameters.AddWithValue("@userId", LoginPage.user_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                User_Bank_Id = reader.GetInt32(0);
                                Console.WriteLine($"Банк не найден, Банку присвоено значение {User_Bank_Id}");
                            }
                        }
                    }
                }

                query = "INSERT INTO [Order] (Type_Insurance_Id, User_Type_Id, User_Id, Insurance_Object, Filing_Date, User_Bank_Id) VALUES( @Type_Insurance_Id, @User_Type_Id, @User_Id, @Insurance_Object, @Filing_Date, @User_Bank_Id);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Type_Insurance_Id", selectedIndex + 1); // Вместо Логин пишем то, что в Текстбоксе, и так далее
                    command.Parameters.AddWithValue("@User_Id", LoginPage.user_id); // Вместо ЮсерТайпИд пишем 1
                    command.Parameters.AddWithValue("@User_Type_Id", user_type_id);
                    command.Parameters.AddWithValue("@Insurance_Object", Insurance_Object.Text);
                    command.Parameters.AddWithValue("@Filing_Date", DateTime.Now);
                    command.Parameters.AddWithValue("@User_Bank_Id", User_Bank_Id);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Ваша заявка была принята в работу, скоро с вами свяжется менеджер", "Успех!");
                    Manager.MainFrame.GoBack();
                }
            }
        }
    }
}
