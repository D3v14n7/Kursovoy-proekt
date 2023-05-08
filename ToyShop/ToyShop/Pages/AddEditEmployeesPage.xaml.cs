using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FIOLib;
using PhoneLib;
using ToyShop.Entities;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы добавления/редактирования сотрудника
    /// </summary>
    public partial class AddEditEmployeesPage : Page
    {
        /// <summary>
        /// Создание переменной для присвоения конкретного поля таблицы Employee
        /// </summary>
        private Entities.Employee _currentEmployee = null;

        public AddEditEmployeesPage()
        {
            InitializeComponent();
        }       

        /// <summary>
        /// Отображение данных конкретного сотрудника при его редактирования
        /// </summary>
        public AddEditEmployeesPage(Entities.Employee employee)
        {
            InitializeComponent();

            _currentEmployee = employee;
            Title = "Редактировать сотрудника";
            TBoxSurname.Text = _currentEmployee.Surname_employee;
            TBoxName.Text = _currentEmployee.Name_employee;
            TBoxPatronymic.Text = _currentEmployee.Patronymic_employee;
            TBoxPhone.Text = _currentEmployee.Phone_number;
            //Отображение должности сотрудника в зависимости от выбранного элемента ComboBox
            if (_currentEmployee.Id_position != null)
            {
                for (int i = 0; i <= ComboPosition.Items.Count; i++)
                {
                    if (i == _currentEmployee.Id_position)
                    {
                        ComboPosition.SelectedIndex = i - 1;
                        break;
                    }
                }

            }
        }
        /// <summary>
        /// Обработчик ошибок в заполнении данных при добавлении/редактировании сотрудника
        /// </summary>
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxSurname.Text)) errorBuilder.AppendLine("Фамилия должна быть заполнена;");
            {
                if (!FIO.NameСorrectness(TBoxSurname.Text)) errorBuilder.AppendLine("Некорректная фамилия");
            }

            if (string.IsNullOrWhiteSpace(TBoxName.Text)) errorBuilder.AppendLine("Имя должно быть заполнено;");
            {
                if (!FIO.NameСorrectness(TBoxName.Text)) errorBuilder.AppendLine("Некорректное имя");
            }

            if (string.IsNullOrWhiteSpace(TBoxPatronymic.Text)) errorBuilder.AppendLine("Фамилия должна быть заполнена;");
            {
                if (!FIO.NameСorrectness(TBoxPatronymic.Text)) errorBuilder.AppendLine("Некорректное отчество");
            }
            if (string.IsNullOrWhiteSpace(TBoxPhone.Text)) errorBuilder.AppendLine("Номер телефона должен быть заполнен;");
            {
                if (!Phone.PhoneCorrectness(TBoxPhone.Text)) errorBuilder.AppendLine("Некорректнный номер телефона");
            }

            var employeeFromDB = App.Context.Employees.ToList().FirstOrDefault(p => p.Surname_employee.ToLower() == TBoxSurname.Text.ToLower() && p.Name_employee.ToLower() == TBoxName.Text.ToLower() && p.Patronymic_employee.ToLower() == TBoxPatronymic.Text.ToLower());
            if (employeeFromDB != null && employeeFromDB != _currentEmployee) errorBuilder.AppendLine("Такой сотрудник уже есть в базе;");

            if (string.IsNullOrEmpty(ComboPosition.Text)) errorBuilder.AppendLine("Должность обязательна для заполнения; ");

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();
        }
        /// <summary>
        /// Обработчик события добавления/редактирования сотрудника
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //добавление нового сотрудника
                if (_currentEmployee == null)
                {
                    var employee = new Entities.Employee
                    {
                        Surname_employee = TBoxSurname.Text,
                        Name_employee = TBoxName.Text,
                        Patronymic_employee = TBoxPatronymic.Text,
                        Phone_number = TBoxPhone.Text,
                        Id_position = ComboPosition.SelectedIndex + 1,

                    };
                    App.Context.Employees.Add(employee);
                    App.Context.SaveChanges();
                    MessageBox.Show("Сотрудник успешно добавлен!");
                }
                //редактирование существующего сотрудника
                else
                {
                    _currentEmployee.Surname_employee = TBoxSurname.Text;
                    _currentEmployee.Name_employee = TBoxName.Text;  
                    _currentEmployee.Patronymic_employee = TBoxPatronymic.Text;
                    _currentEmployee.Phone_number = TBoxPhone.Text;
                    _currentEmployee.Id_position = ComboPosition.SelectedIndex + 1;
                    App.Context.SaveChanges();
                    MessageBox.Show("Сотрудник успешно изменен!");
                }
                NavigationService.GoBack();
            }
        }
    }
}
