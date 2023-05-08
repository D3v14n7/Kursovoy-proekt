using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ToyShop.Entities;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы сотрудников
    /// </summary>
    public partial class EmployeesPage : Page
    {
        /// <summary>
        /// Отображение значений по умолчанию при входе на страницу
        /// </summary>
        public EmployeesPage()
        {
            InitializeComponent();
            LViewEmployees.ItemsSource = App.Context.Employees.ToList();//вывод всех сотрудников из базы данных
            ComboSortBy.SelectedIndex = 0;//начальный элемент ComboBox порядка сотрудников
            UpdateEmployees();
        }
        /// <summary>
        /// Переход на страницу добавления сотруднка
        /// </summary>
        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditEmployeesPage());
        }
        /// <summary>
        /// Метод отображения сотрудников 
        /// </summary>
        private void UpdateEmployees()
        {
            var employee = App.Context.Employees.ToList();
            //сортировка сотрудников по алфовиту по фамилии
            if (ComboSortBy.SelectedIndex == 1)
                employee = employee.OrderBy(p => p.Surname_employee).ToList();
            if (ComboSortBy.SelectedIndex == 2)
                employee = employee.OrderByDescending(p => p.Surname_employee).ToList();
            //поиск сотрудников по имени или фамилии или отчеству
            employee = employee.Where(p => p.Surname_employee.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Name_employee.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Patronymic_employee.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewEmployees.ItemsSource = employee;//вывод сотрудников в зависимости от сортировки и поиска
        }
        /// <summary>
        /// Переход на страницу редактирования конуретного сотрудника
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentEmployee = (sender as Button).DataContext as Entities.Employee;
            NavigationService.Navigate(new AddEditEmployeesPage(currentEmployee));
        }
        /// <summary>
        /// Удаление конкретного сотрудника
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentEmployee = (sender as Button).DataContext as Entities.Employee;
            var currentEmployeeInPurchase = App.Context.Employees.Where(p => p.Id_employee == currentEmployee.Id_employee).FirstOrDefault();
            Purchase employeeInPurchase = App.Context.Purchases.Where(p => p.Id_employee == currentEmployeeInPurchase.Id_employee).FirstOrDefault();

            if (MessageBox.Show($"Вы уверены, что хотите удалить сотрудника: " + $"{currentEmployee.Surname_employee} {currentEmployee.Name_employee} {currentEmployee.Patronymic_employee}? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.Context.Employees.Remove(currentEmployee);
                App.Context.SaveChanges();
                employeeInPurchase.Id_employee = null;
                App.Context.SaveChanges();
                UpdateEmployees();
            }
        }
        /// <summary>
        /// При выборе элемента сортировки вызывается метод отображения сотрудников
        /// </summary>
        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEmployees();
        }
        /// <summary>
        /// При введение в поиск вызывается метод отображения сотрудников
        /// </summary>
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEmployees();
        }
        /// <summary>
        /// Вызов метода отображения сотрудников при загрузке страницы
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateEmployees();
        }
    }
}
