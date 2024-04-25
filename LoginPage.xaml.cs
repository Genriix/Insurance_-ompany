using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string log = Login.Text.ToLower();

            if (log == "user" && Password.Password == "user" && CheckSequence())
            {
                Login.Text = "";
                Manager.MainFrame.Navigate(new UserPage());
            }
            else { }
        }

        private void GenerateRandomSequence(object sender, RoutedEventArgs e)
        {
            string randomSequence = GenerateRandomString(6);
            CapOut.Text = randomSequence;
            CapOut.IsEnabled = false;
        }

        private bool CheckSequence()
        {
            if (CapOut.Text == CapIn.Text)
            {
                CapOut.Text = "";
                CapIn.Text = "";
                return true;
            }
            else
            {
                CapOut.Text = "";
                CapIn.Text = "";
                return false;
            }
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }

            return sb.ToString();
        }
    }
}
