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
    /// Логика взаимодействия для Polices.xaml
    /// </summary>
    public partial class Polices : Page
    {
        public Polices()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(Manager.connectionString))
            {
                connection.Open();
                string query = "" +
                    "SELECT " +
                    "Insurance_Policy.id, " +
                    "Type_Insurance.Name, " +
                    "User_Type.UserTypeName, " +
                    "Insurance_Policy.Insurance_Object, " +
                    "Insurance_Policy.Date_Of_Conclusion " +
                    "FROM Insurance_Policy " +
                    "JOIN Type_Insurance ON Insurance_Policy.Type_Insurance_Id = Type_Insurance.id " +
                    "JOIN [User] ON Insurance_Policy.User_Id = [User].id " +
                    "JOIN User_Type ON [User].User_Type_Id = User_Type.id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (DataTable dataTable = new DataTable())
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            try
                            {
                                adapter.Fill(dataTable);

                                Policy.ItemsSource = dataTable.DefaultView;
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
        public static string selectedPolicy;
        private void BtnPolicy_Click(object sender, RoutedEventArgs e)
        {
            if (Policy.SelectedItem != null)
            {
                DataRowView row = (DataRowView)Policy.SelectedItem;
                selectedPolicy = row["id"].ToString();
            }
            Manager.MainFrame.Navigate(new PolicyPage());
        }

    }
}
