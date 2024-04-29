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
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Insurance_сompany
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private CAPTCHA captcha = new CAPTCHA(); // Инициализируем экземпляр класса капча
        public RegistrationPage()
        {
            InitializeComponent();
            CapOut.IsEnabled = false; // Делаем текстбокс не активным
            captcha.CaptchaIsGenerate = false; // Мы не проходили капчу
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-5CVQU3F\\SQLEXPRESS;Initial Catalog=InsuranceCompany;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    /// Ошибочки
                    /*
                    StringBuilder errors = new StringBuilder(); // Инициализируем экземпляр класса стрингБилдер
                    var converter = new System.Windows.Media.BrushConverter();
                    var lightGray = (Brush)converter.ConvertFromString("#FFABADB3");

                    if (Login.Text.Length < 8 && Password.Text.Length >= 8)
                    {
                        Login.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Логин меньше 8 символов");
                    }
                    else if (Login.Text.Length >= 8 && Password.Text.Length < 8)
                    {
                        Login.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Пароль меньше 8 символов");
                    }

                    else if (Login.Text.Length < 8 && Password.Text.Length < 8)
                    {
                        Login.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Логин и пароль меньше 8 символов");
                    }


                    string query = "SELECT COUNT(*) FROM User WHERE Login = @Login";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", Login.Text);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            Login.BorderBrush = System.Windows.Media.Brushes.Red;
                            errors.AppendLine("Логин занят другим пользователем");
                        }
                    }


                    bool containsDigit = Password.Text.Any(char.IsDigit);
                    bool containsLetter = Password.Text.Any(char.IsLetter);

                    if (!(containsDigit && containsLetter))
                    {
                        Password.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Пароль должен содержать латинские цифры и буквы");
                    }
                    else if (!IsAlphaNumeric(Password.Text))
                    {
                        Password.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Пароль должен состоять только из цифр и букв.");
                    }

                    if (Password.Text != Rep_Password.Text)
                    {
                        Password.BorderBrush = System.Windows.Media.Brushes.Red;
                        Rep_Password.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Пароли должны совпадать");
                    }

                    if (string.IsNullOrEmpty(F_Name.Text))
                    {
                        F_Name.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Введите имя");
                    }
            
                    else if (!Regex.IsMatch(F_Name.Text, @"^[А-Яа-я]+$"))
                    {
                        F_Name.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Имя должно содержать только буквы");
                    }

                    if (string.IsNullOrEmpty(L_Name.Text))
                    {
                        L_Name.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Введите фамилию");
                    }

                    else if (!Regex.IsMatch(L_Name.Text, @"^[А-Яа-я]+$"))
                    {
                        L_Name.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Фамилия должно содержать только буквы");
                    }

                    if (!Regex.IsMatch(M_Name.Text, @"^[А-Яа-я]+$") && !string.IsNullOrEmpty(M_Name.Text))
                    {
                        M_Name.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Отчество должно содержать только буквы");
                    }
            
                    captcha.CapOut = CapOut.Text.ToString();
                    captcha.CapIn = CapIn.Text.ToString();

                    if (captcha.CaptchaIsGenerate == false)
                    {
                        CapIn.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Пройдите тест CAPTCHA");
                    }
                    else if (captcha.CheckSequence() != true)
                    {
                        CapIn.BorderBrush = System.Windows.Media.Brushes.Red;
                        errors.AppendLine("Повторите тест CAPTCHA");
                    }
                    else { CapIn.BorderBrush = lightGray; }

                    if (errors.Length > 0) //Выводи ошибки если есть
                    {
                        MessageBox.Show(errors.ToString(), "Ошибка регистрации");
                        CapOut.Text = "";
                        CapIn.Text = "";
                        return; // Завершаем исполнение метода и дальше по коду не идём
                    }
                    */
                    /// Ошибочки кончились

                    // Создание SQL-запроса с параметрами
                    string query = "INSERT INTO [User] (User_Type_Id, Login, Password, Telephone_Number) VALUES (@UserTypeId, @Login, @Password, @Telephone_Number)";

                    // Подготовка команды с параметрами
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserTypeId", 1);
                        command.Parameters.AddWithValue("@Login", Login.Text);
                        command.Parameters.AddWithValue("@Password", Password.Text);
                        command.Parameters.AddWithValue("@Telephone_Number", T_Number.Text);

                        // Выполнение команды
                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"Добавлено {rowsAffected} записей в таблицу IndividualUser.");
                    }
                }
                MessageBox.Show("Вы успешно зарегистрировались!", "Успех!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }

            CapOut.Text = "";
            CapIn.Text = "";
            captcha.CaptchaIsGenerate = false;
            Manager.MainFrame.GoBack();
        }
        private void GenerateRandomSequence(object sender, RoutedEventArgs e)
        {
            CapOut.Text = captcha.GenerateRandomSequence(); //Записываем в наш текстбокс то, что скажет капча из экземпляра класса
        }
        static bool IsAlphaNumeric(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
        }
    }
}
/*
 
  private static InsuranceCompanyEntities _context;
  
  public static InsuranceCompanyEntities GetContext()
        {
            if (_context == null)
                _context = new InsuranceCompanyEntities();
            return _context;
        }

  */