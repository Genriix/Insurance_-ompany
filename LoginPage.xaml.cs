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
            CaptureIsGenerate = false; // Мы не проходили капчу
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder(); // Инициализируем экземпляр класса стрингБилдер

            /// Заполняем эклемпляр ошибками если есть

            if (Login.Text.ToLower() != "user") errors.AppendLine("Неправильное имя пользователя");
            if (Password.Password != "user") errors.AppendLine("Неправильный пароль");
            if (CaptureIsGenerate == false) errors.AppendLine("Пройдите тест CAPTCHA");
            else if (CheckSequence() != true) errors.AppendLine("Повторите тест CAPTCHA") ;
            if (errors.Length > 0) //Выводи ошибки если есть
            {
                MessageBox.Show(errors.ToString(), "Пiшов нахуй москаль ебучий" );
                CapOut.Text = "";
                return; // Завершаем исполнение метода
            }

            /// Если всё ок, и мы не попались на ловушку ошибок, то отчищаем поля и переходим на следующую страницу

            Login.Text = "";
            Password.Password = "";
            Manager.MainFrame.Navigate(new UserPage());
            CapOut.Text = "";
            CapIn.Text = "";
            CaptureIsGenerate = false;
        }


        /// Реализация капчи

        private bool CaptureIsGenerate { get; set; } // Создаём приватное поле с типом данных true/false
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Создаём константу с буковами и цыферами
            StringBuilder sb = new StringBuilder(); // Инициализируем экземпляр класса СтрингБилдер
            Random random = new Random(); // Инициализируем экземпляр класса Рандом

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]); // Добавляем в экземпляр класса стирнгБилдер заданное количество случайных цыфарок и буковок
            }
            CaptureIsGenerate = true;
            return sb.ToString(); // Возвращаем строковую случайную последовательность символов
        }
        private void GenerateRandomSequence(object sender, RoutedEventArgs e)
        {
            string randomSequence = GenerateRandomString(6); // Инициализируем экемпляр функции генерации случайной последовательности символов
            CapOut.Text = randomSequence; // Записываем экземпляр функции в текстбокс
        }

        private bool CheckSequence()
        {
            /// Сравниваем ордин тестбокс с другим
            if (CapOut.Text == CapIn.Text && CaptureIsGenerate) {  return true; } // Если да, то возвращаем тру
            else { return false; } // Если нет, то нет
        }
    }
}
