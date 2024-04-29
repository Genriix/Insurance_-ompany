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

namespace Insurance_сompany
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public User _currentUser = new User();
        public static int UserId { get; set; }
        public static string FName { get; set; }
        public static string LName { get; set; }
        public static string MName { get; set; }
        public static int PassportNum { get; set; }
        public static int BurthDate { get; set; }



        private CAPTCHA captcha = new CAPTCHA(); // Инициализируем экземпляр класса капча
        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = _currentUser;
            CapOut.IsEnabled = false; // Делаем текстбокс не активным
            captcha.CaptchaIsGenerate = false; // Мы не проходили капчу
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder(); // Инициализируем экземпляр класса стрингБилдер
            var converter = new System.Windows.Media.BrushConverter();
            var lightGray = (Brush)converter.ConvertFromString("#FFABADB3");

            /// Приравниваем публичные поля КапИн и КУапАут и 
            /// нашим КапИн и КУапАут (текстбоксы вход и выход качи)

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
                MessageBox.Show(errors.ToString(), "Ошибка входа");
                CapOut.Text = "";
                CapIn.Text = "";
                return; // Завершаем исполнение метода и дальше по коду не идём
            }

            if (_currentUser.id == 0)
            {
                _currentUser.User_Type_Id = 1;
                InsuranceCompanyEntities.GetContext().User.Add(_currentUser);
                FName = F_Name.Text;
                LName = L_Name.Text;
                MName = M_Name.Text;
                PassportNum = int.Parse(N_Pass.Text);
                BurthDate = int.Parse(B_Date.Text);
            }

            try
            {
                InsuranceCompanyEntities.GetContext().SaveChanges();
                UserId = _currentUser.id;
                IndividualRegstration.Individual_Regstration();
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
    }

    public static class IndividualRegstration
    {
        public static void Individual_Regstration() 
        {
            string connectionString = "Data Source=DESKTOP-5CVQU3F\\SQLEXPRESS;Initial Catalog=InsuranceCompany;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Создание SQL-запроса с параметрами
                string query = "INSERT INTO Individual_User (User_Id, F_Name, L_Name, M_Name, Passport_Num, Burth_Date) VALUES (@UserId, @FName, @LName, @MName, @PassportNum, @BurthDate)";

                // Подготовка команды с параметрами
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", RegistrationPage.UserId);
                    command.Parameters.AddWithValue("@FName", RegistrationPage.FName);
                    command.Parameters.AddWithValue("@LName", RegistrationPage.LName);
                    command.Parameters.AddWithValue("@MName", RegistrationPage.MName);
                    command.Parameters.AddWithValue("@PassportNum", RegistrationPage.PassportNum);
                    command.Parameters.AddWithValue("@BurthDate", RegistrationPage.BurthDate);

                    // Выполнение команды
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"Добавлено {rowsAffected} записей в таблицу IndividualUser.");
                }
            }
        }
    }
}
/*
 * private static InsuranceCompanyEntities _context;
 * 
 * public static InsuranceCompanyEntities GetContext()
        {
            if (_context == null)
                _context = new InsuranceCompanyEntities();
            return _context;
        }
 * 
 * 
 * 
 * 
 * 
 * */