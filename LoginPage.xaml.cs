using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
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
        private CAPTCHA captcha = new CAPTCHA(); // Инициализируем экземпляр класса капча

        public static int user_id; // Создаём Публичную переменную с id пользователя 

        public LoginPage()
        {
            InitializeComponent();
            CapOut.IsEnabled = false; // Делаем текстбокс не активным
            captcha.CaptchaIsGenerate = false; // Мы не проходили капчу
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /// Приравниваем публичные поля КапИн и КапАут и 
            /// нашим КапИн и КУапАут (текстбоксы вход и выход капчи)

            string connectionString = "Data Source=DESKTOP-5CVQU3F\\SQLEXPRESS;Initial Catalog=InsuranceCompany;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Открывает наш коннект
                StringBuilder errors = new StringBuilder(); // Инициализируем экземпляр класса стрингБилдер
                var converter = new System.Windows.Media.BrushConverter();
                var lightGray = (Brush)converter.ConvertFromString("#FFABADB3");

                string query = "SELECT id FROM [User] WHERE Login = @Login AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", Login.Text);
                    command.Parameters.AddWithValue("@Password", Password.Password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user_id = reader.GetInt32(0); // Получаем значение столбца id
                        }
                        else if (!reader.Read())
                        {
                            Login.BorderBrush = System.Windows.Media.Brushes.Red;
                            Password.BorderBrush = System.Windows.Media.Brushes.Red;
                            errors.AppendLine("Пользователь не найден");
                        }
                        else
                        {
                            Login.BorderBrush = lightGray;
                            Password.BorderBrush = lightGray;
                        }
                    }

                if (captcha.CaptchaIsGenerate == false) // Капча не была сгенерирована
                { 
                    CapIn.BorderBrush = System.Windows.Media.Brushes.Red;
                    errors.AppendLine("Пройдите тест CAPTCHA"); 
                }
                else if (captcha.CheckSequence() != true) // Или капча была пройдена не верно
                {
                    CapIn.BorderBrush = System.Windows.Media.Brushes.Red;
                    errors.AppendLine("Повторите тест CAPTCHA"); 
                }
                else { CapIn.BorderBrush = lightGray; }

                if (errors.Length > 0) //Выводи ошибки если есть
                {
                    MessageBox.Show(errors.ToString(), "Ошибка входа" );
                    CapOut.Text = "";
                    CapIn.Text = "";
                    return; // Завершаем исполнение метода и дальше по коду не идём
                }

                Login.Text = "";
                Password.Password = "";
                CapOut.Text = "";
                CapIn.Text = "";
                captcha.CaptchaIsGenerate = false;
                Manager.MainFrame.Navigate(new UserPage()); 
                }
            }
        }

        private void GenerateRandomSequence(object sender, RoutedEventArgs e)
        {
            CapOut.Text = captcha.GenerateRandomSequence(); //Записываем в наш текстбокс то, что скажет капча из экземпляра класса
        }
    }
}
