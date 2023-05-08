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

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы навигации
    /// </summary>
    public partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            InitializeComponent();

            if (App.CurrentUser.RoleId == 1)//если зашел администратор - отображать кнопку для перехода на страницу сотрудников
            {
                BtnEmployeesPage.Visibility = Visibility.Visible;
            }
            else//если зашел не администратор - не отображать кнопку для перехода на страницу сотрудников
            {
                BtnEmployeesPage.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Переход на страницу игрушек
        /// </summary>
        private void BtnToysPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToysPage());
        }
        /// <summary>
        /// Переход на страницу сотрудников
        /// </summary>
        private void BtnEmployeesPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesPage());
        }
        /// <summary>
        /// Переход на страницу покупок
        /// </summary>
        private void BtnPurchasePage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PurchasePage());
        }
    }
}
