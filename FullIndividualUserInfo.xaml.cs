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
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                if (MessageBox.Show("Вы уверены, что хотите изменить даннные?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    connection.Open();
                    string query = "" +
                        "UPDATE Individual_User SET " +
                        "F_Name = @FName, " +
                        "L_Name = @LName, " +
                        "M_Name = @MName, " +
                        "Passport_Num = @PNum, " +
                        "Burth_Date = @BDate " +
                        "WHERE User_Id = @UserId;" +
                        "UPDATE [User] SET " +
                        "Login = @Login, " +
                        "Password = @Password, " +
                        "Telephone_Number = @TNum, " +
                        "Address = @Address " +
                        "WHERE id = @UserId;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FName", F_Name.Text);
                        command.Parameters.AddWithValue("@LName", L_Name.Text);
                        command.Parameters.AddWithValue("@MName", M_Name.Text);
                        command.Parameters.AddWithValue("@Login", Login.Text);
                        command.Parameters.AddWithValue("@TNum", T_Num.Text);
                        command.Parameters.AddWithValue("@Address", Address.Text);
                        command.Parameters.AddWithValue("@PNum", P_Num.Text);
                        command.Parameters.AddWithValue("@BDate", B_Date.Text);
                        command.Parameters.AddWithValue("@Password", Password.Password);
                        command.Parameters.AddWithValue("@UserId", LoginPage.user_id);
                        command.Parameters.AddWithValue("@id", LoginPage.user_id);

                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Данные были обновлены!", "Успех!");
                }
            }
        }
    }
}
