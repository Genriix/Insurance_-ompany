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
            CapOut.IsEnabled = false; // Делаем текстбокс не активным
            CaptchaIsGenerate = false; // Мы не проходили капчу
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder(); // Инициализируем экземпляр класса стрингБилдер

            /// Заполняем эклемпляр ошибками если есть

            var converter = new System.Windows.Media.BrushConverter();
            var lightGray = (Brush)converter.ConvertFromString("#FFABADB3");

            if (Login.Text.ToLower() != "user")
            {
                Login.BorderBrush = System.Windows.Media.Brushes.Red;
                errors.AppendLine("Неправильное имя пользователя");
            }
            else { Login.BorderBrush = lightGray; }

            if (Password.Password != "user")
            {
                Password.BorderBrush = System.Windows.Media.Brushes.Red;
                errors.AppendLine("Неправильный пароль"); 
            }
            else { Password.BorderBrush = lightGray; }

            if (CaptchaIsGenerate == false) 
            { 
                CapIn.BorderBrush = System.Windows.Media.Brushes.Red;
                errors.AppendLine("Пройдите тест CAPTCHA"); 
            }
            else if (CheckSequence() != true) 
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

            /// Если всё ок, и мы не попались на ловушку ошибок, то отчищаем поля и переходим на следующую страницу

            Login.Text = "";
            Password.Password = "";
            CapOut.Text = "";
            CapIn.Text = "";
            CaptchaIsGenerate = false;
            Manager.MainFrame.Navigate(new UserPage());
        }


        /// Реализация капчи

        private bool CaptchaIsGenerate { get; set; } // Создаём приватное поле с типом данных true/false
        private string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789"; // Создаём константу с буковами и цыферами
            StringBuilder sb = new StringBuilder(); // Инициализируем экземпляр класса СтрингБилдер
            Random random = new Random(); // Инициализируем экземпляр класса Рандом

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]); // Добавляем в экземпляр класса стирнгБилдер заданное количество случайных цыфарок и буковок
            }
            CaptchaIsGenerate = true;
            return sb.ToString(); // Возвращаем строковую случайную последовательность символов
        }
        private void GenerateRandomSequence(object sender, RoutedEventArgs e)
        {
            string randomSequence = GenerateRandomString(6); // Инициализируем экемпляр функции генерации случайной последовательности символов
            CapOut.Text = randomSequence; // Записываем экземпляр функции в текстбокс
        }

        private bool CheckSequence()
        {
            /// Сравниваем один тестбокс с другим
            if (CapOut.Text == CapIn.Text) {  return true; } // Если да, то возвращаем тру
            else { return false; } // Если нет, то нет
        }
    }
}
