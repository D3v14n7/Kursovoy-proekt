using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using PasswordLib;
using LoginLib;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы регистрации
    /// </summary>
    public partial class RegistrationPage : Page
    {
        /// <summary>
        /// Создание  переменной для присвоения конкретного поля таблицы User
        /// </summary>
        private Entities.User _currentUser = null;
        public RegistrationPage()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Присвоение переменной пользователей для сравнения логинов
        /// </summary>
        public RegistrationPage (Entities.User пользователь)
        {
            InitializeComponent();

            _currentUser = пользователь;
            TBoxLogin.Text = _currentUser.Login;
        }
        /// <summary>
        /// Обработчик ошибок в заполнении данных при регистрации
        /// </summary>
        private string CheckErrors()
        {

            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxLogin.Text)) errorBuilder.AppendLine("Логин обязателен для заполнения;");
            {
                if (!Login.LoginCorrectness(TBoxLogin.Text)) errorBuilder.AppendLine("Некорректный логин");
            }

            if (string.IsNullOrWhiteSpace(PBoxPassword.Password)) errorBuilder.AppendLine("Пароль обязателен для заполнения;");


            var userFromDB = App.Context.Users.ToList().FirstOrDefault(p => p.Login.ToLower() == TBoxLogin.Text.ToLower());
            if (_currentUser != userFromDB && userFromDB != null)
            {
                errorBuilder.AppendLine("Такой пользователь уже есть в базе данных");
            }

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();

        }
        /// <summary>
        /// Обработчик события кнопки регистрации
        /// </summary>
        private void BtnRegistrate_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                var errorMessage = CheckErrors();
                if (errorMessage.Length > 0)
                {
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else//проверка сложности пароля, если пароль хороший или сложный - зарегистрироваться
                {
                    if (_currentUser == null && (TBoxPasswordComplexity.Text == "Хороший") || (TBoxPasswordComplexity.Text == "Сложный"))
                    {
                        if (PBoxPassword.Password == PBoxPasswordConfirm.Password)//проверка совпадений паролей в поле повторения пароля, если пароли совпадают - зарегистрироваться
                        {
                            Entities.User user = new Entities.User();
                            user.Login = TBoxLogin.Text;
                            user.Password = PBoxPassword.Password;
                            user.RoleId = 3;
                            App.Context.Users.Add(user);
                            MessageBox.Show("Вы успешно зарегистрировались!");
                            App.Context.SaveChanges();
                            NavigationService.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Пароль должен быть сложнее", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show($"Возникла ошибка подключения к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик события изменения поля пароля, для выяснения его сложности
        /// </summary>
        private void PBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordCheck = Password.PasswordComplexity(PBoxPassword.Password);
            TBoxPasswordComplexity.Text = passwordCheck;//поле для вывода сложности пароля
        }
    }
}
