using FIOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ToyShop.Entities;
using PhoneLib;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы добавления покупки
    /// </summary>
    public partial class AddPurchasePage : Page
    {
        public AddPurchasePage()
        {
            InitializeComponent();
            ComboEmp();
        }
        public List<Employee> Emp { get; set; }//Создание списка сотрудников

        /// <summary>
        /// Вывод в комбобокс сотрудников из списка с должностью "кассир"
        /// </summary>
        private void ComboEmp()
        {
            Toys_shopEntities employee = new Toys_shopEntities();
            var sotr = employee.Employees.Where(p => p.Id_position == 2).ToList();
            Emp = sotr;
            DataContext = Emp;
        }
        /// <summary>
        /// Обработчик ошибок в заполнении данных при добавлении покупки
        /// </summary>
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxSurname.Text)) errorBuilder.AppendLine("Фамилия должна быть заполнена;");
                if (!FIO.NameСorrectness(TBoxSurname.Text)) errorBuilder.AppendLine("Некорректная фамилия");

            if (string.IsNullOrWhiteSpace(TBoxName.Text)) errorBuilder.AppendLine("Имя должно быть заполнено;");
                if (!FIO.NameСorrectness(TBoxName.Text)) errorBuilder.AppendLine("Некорректное имя");

            if (string.IsNullOrWhiteSpace(TBoxPatronymic.Text)) errorBuilder.AppendLine("Фамилия должна быть заполнена;");
                if (!FIO.NameСorrectness(TBoxPatronymic.Text)) errorBuilder.AppendLine("Некорректное отчество");

            if (string.IsNullOrWhiteSpace(TboxPhone.Text)) errorBuilder.AppendLine("Номер телефона обязателен для заполнения;");
                if (!Phone.PhoneCorrectness(TboxPhone.Text)) errorBuilder.AppendLine("Некорректный номер телефона");

            if (string.IsNullOrWhiteSpace(Date.Text)) errorBuilder.AppendLine("Дата обязательна для заполнения;");

            decimal cost = 0;
            if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost <= 0) errorBuilder.AppendLine("Стоимость покупки должна быть положительным числом;");

            if (string.IsNullOrEmpty(ComboEmployee.Text)) errorBuilder.AppendLine("Сотрудник обязателен для выбора; ");

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();
        }
        /// <summary>
        /// Обработчик события добавления покупки
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //добавление покупки и клиента
            else
            {
                Buyer buyer = new Buyer();
                buyer.Name_buyer = TBoxName.Text;
                buyer.Surname_buyer = TBoxSurname.Text;
                buyer.Patronymic_buyer = TBoxPatronymic.Text;
                buyer.Phone_number = TboxPhone.Text;
                App.Context.Buyers.Add(buyer);
                var idPurchase = App.Context.Purchases.OrderByDescending(p => p.Id_purchase).FirstOrDefault();
                DateTime date;
                DateTime.TryParse(Date.Text, out date); 
                //добавление покупки
                var purchase = new Entities.Purchase
                {
                    Id_purchase = idPurchase.Id_purchase + 1,
                    Id_buyer = buyer.Id_buyer,
                    Cost = (int)decimal.Parse(TBoxCost.Text),
                    Id_employee = Emp[ComboEmployee.SelectedIndex].Id_employee,//внесение сотрудника в покупку, в зависимости от выбранного элемента ComboBox
                    Date_purchase = date,
                };
                App.Context.Purchases.Add(purchase);
                App.Context.SaveChanges();
                MessageBox.Show("Покупка успешно добавлена!");
                NavigationService.GoBack();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Date.SelectedDate = DateTime.Today;
        }
    }
}
