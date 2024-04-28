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
    /// Логика взаимодействия для LegalRegistration.xaml
    /// </summary>
    public partial class LegalRegistration : Page
    {
        private CAPTCHA captcha = new CAPTCHA(); // Инициализируем экземпляр класса капча
        public LegalRegistration()
        {
            InitializeComponent();
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

            CapOut.Text = "";
            CapIn.Text = "";
            captcha.CaptchaIsGenerate = false;
            MessageBox.Show("Вы успешно зарегистрировались!", "Успех!");
            Manager.MainFrame.GoBack();
        }
        private void GenerateRandomSequence(object sender, RoutedEventArgs e)
        {
            CapOut.Text = captcha.GenerateRandomSequence(); //Записываем в наш текстбокс то, что скажет капча из экземпляра класса
        }

    }
}
