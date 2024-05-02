using System;
using System.Collections.Generic;
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

namespace Insurance_сompany
{
    /// <summary>
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        public ManagerPage()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();
                string query = "" +
                    "SELECT " +
                    "Type_Insurance.Name, " +
                    "User_Type.UserTypeName, " +
                    "[Order].Insurance_Object, " +
                    "[Order].Filing_Date " +
                    "FROM [Order] " +
                    "JOIN Type_Insurance ON [Order].Type_Insurance_Id = Type_Insurance.id " +
                    "JOIN [User] ON [Order].User_Id = [User].id " +
                    "JOIN User_Type ON [User].User_Type_Id = User_Type.id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", LoginPage.user_id);

                    using (DataTable dataTable = new DataTable())
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
            }
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
