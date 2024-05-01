using System;
using System.Collections.Generic;
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

namespace Insurance_сompany
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
        }

        private void UserIPage_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();
                string query = "SELECT User_Type_Id FROM [User] WHERE id = @user_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", LoginPage.user_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            if (reader.GetInt32(0) == 1)
                            {
                                Manager.MainFrame.Navigate(new UserInfoPage());
                            }
                            else if (reader.GetInt32(0) == 2)
                            {
                                Manager.MainFrame.Navigate(new LegalUserInfoPage());
                            }
                        }
                    }
                }
            }
        }
    }
}
