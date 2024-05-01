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
    /// Логика взаимодействия для FullIndividualUserInfo.xaml
    /// </summary>
    public partial class FullIndividualUserInfo : Page
    {
        public FullIndividualUserInfo()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();
                string query = "SELECT [User].*, Individual_User.* FROM [User] JOIN Individual_User ON [User].id = Individual_User.User_Id WHERE id = @UserId;";
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
                            Login.Text = reader["Login"].ToString();
                            T_Num.Text = reader["Telephone_Number"].ToString();
                            Address.Text = reader["Address"].ToString();
                            P_Num.Text = reader["Passport_Num"].ToString();
                            B_Date.Text = reader["Burth_Date"].ToString();
                            Password.Password = reader["Password"].ToString();
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
