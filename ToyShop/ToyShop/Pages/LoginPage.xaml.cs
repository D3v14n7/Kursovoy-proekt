using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы входа
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик события кнопки входа
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var errorMessage = CheckErrors();
                if (errorMessage.Length > 0)
                {
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //в случае совпадения данных пользователя - вход в приложение
                else
                {
                    var currentUser = App.Context.Users.FirstOrDefault(p => p.Login == TBoxLogin.Text && p.Password == PBoxPassword.Password);

                    if (currentUser != null)
                    {
                        App.CurrentUser = currentUser;
                        if (currentUser.RoleId == 3) //если входит обычный пользователь - перейти на страницу игрушек 
                        {
                            NavigationService.Navigate(new ToysPage());
                        }
                        else //если входит сотрудник или администратор - перейти на страницу навигации
                        {
                            NavigationService.Navigate(new NavigationPage());
                        }

                    }
                    //предложение зарегистрироваться, если не удалось войти
                    else
                    {
                        var error = MessageBox.Show("Пользователь с такими данными не найден. Желаете зарегистрироваться?", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (error == MessageBoxResult.Yes)
                        {
                            NavigationService.Navigate(new RegistrationPage());
                        }
                        else
                        {
                            TBoxLogin.Text = "";
                            PBoxPassword.Password = "";
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Возникла ошибка подключения к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик ошибок в заполнении данных при входе
        /// </summary>
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxLogin.Text)) errorBuilder.AppendLine("Введите логин;");

            if (string.IsNullOrWhiteSpace(PBoxPassword.Password)) errorBuilder.AppendLine("Введите пароль;");
            var userFromDB = App.Context.Users.ToList().FirstOrDefault(p => p.Login.ToLower() == TBoxLogin.Text.ToLower());

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();

        }
        /// <summary>
        /// Переход на страницу регистрации
        /// </summary>
        private void LinkRegistrate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
