using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using ToyShop.Entities;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы добавления/редактирования игрушки
    /// </summary>
    public partial class AddEditPage : Page
    {
        /// <summary>
        /// Создание  переменной для присвоения конкретного поля таблицы Toy
        /// </summary>
        private Entities.Toy _currentToy = null;
        /// <summary>
        /// Создание переменной для выбора картинки
        /// </summary>
        private byte[] _mainImageData;
        public AddEditPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Отображение данных конкретной игрушки при ее редактировании
        /// </summary>
        public AddEditPage (Entities.Toy toys)
        {
            InitializeComponent();

            _currentToy = toys;
            Title = "Редактировать игрушку";
            TBoxTitle.Text = _currentToy.Name;
            TBoxCost.Text = _currentToy.Cost.ToString("N2");
            //отображение категории в зависимости от выбранного элемента ComboBox
            if (_currentToy.Id_category != null)
            {
                for (int i = 0; i <= ComboCategory.Items.Count; i++)
                {
                    if (i == _currentToy.Id_category)
                    {
                        ComboCategory.SelectedIndex = i - 1;
                        break;
                    }
                }
                    
            }
            
            var count = App.Context.Toys_on_warehouse.Where(p => p.Id_toy == _currentToy.Id_toy).FirstOrDefault();
            var warehouse = App.Context.Warehouses.Where(p => p.Id_warehouse == count.Id_warehouse && count.Id_toy == _currentToy.Id_toy).FirstOrDefault();
            ComboWarehouse.IsEnabled = false;
            //отображение склада в зависимости от выбранного элемента ComboBox
            for (int i = 0; i <= ComboWarehouse.Items.Count; i++)
            {
                if (i == warehouse.Id_warehouse)
                {
                    ComboWarehouse.SelectedIndex = i - 1;
                    break;
                }
            }
            //отображение количества игрушек
            if (count.Amount_of_toys_on_warehouse >= 0)
                TBoxAmount.Text = count.Amount_of_toys_on_warehouse.ToString();
            else
                TBoxAmount.Text = 0.ToString();
            //отображение стоимости игрушки в зависимости от наличия скидки
            if (_currentToy.Discount > 0)
                TBoxDiscount.Text = (_currentToy.Discount).ToString();
            else
                TBoxDiscount.Text = 0.ToString();
            if (_currentToy.Image != null)
                ImageToy.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_currentToy.Image);
        }
        /// <summary>
        /// Обработчик ошибок в заполнении данных при добавлении/редактировании игрушки
        /// </summary>
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxTitle.Text)) errorBuilder.AppendLine("Название услуги обязательно заполнения;");

            var toyFromDB = App.Context.Toys.ToList().FirstOrDefault(p => p.Name.ToLower() == TBoxTitle.Text.ToLower());
            if (toyFromDB != null && toyFromDB != _currentToy) errorBuilder.AppendLine("Такая игрушка уже есть в базе данных;");

            decimal cost = 0;
            if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost <= 0) errorBuilder.AppendLine("Стоимость игрушки должна быть положительным числом;");

            if (string.IsNullOrEmpty(ComboCategory.Text)) errorBuilder.AppendLine("Категория обязательна для заполнения; ");

            if (!string.IsNullOrEmpty(TBoxDiscount.Text))
            {
                int discount = 0;
                if (int.TryParse(TBoxDiscount.Text, out discount) == false || discount < 0 || discount > 1000)
                {
                    errorBuilder.AppendLine("Размер скидки - целое число в диапозоне от 0 до 1000 рублей;");
                }
            }
            if (string.IsNullOrEmpty(ComboWarehouse.Text)) errorBuilder.AppendLine("Необходимо выбрать склад; ");

            if (!string.IsNullOrEmpty(TBoxAmount.Text))
            { 
                int amount = 0;
                if (int.TryParse(TBoxAmount.Text, out amount) == false)
                errorBuilder.AppendLine("Количество должно быть целым числом или 0");
            }

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();
        }
        /// <summary>
        /// Метод открытия проводника для выбора картинки
        /// </summary>
        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageToy.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
            }
        }
        /// <summary>
        /// Обработчик события добавления/редактирования игрушки
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
                //добавление игрушки
                if (_currentToy == null)
                {
                    Toys_on_warehouse newToy = new Toys_on_warehouse();//объект промежуточной таблицы для отображения количества игрушек и склада, на котором они находятся
                    var toy = new Entities.Toy
                    {
                        Name = TBoxTitle.Text,
                        Cost = (int)decimal.Parse(TBoxCost.Text),
                        Id_category = ComboCategory.SelectedIndex + 1,
                        Discount = (int)(string.IsNullOrWhiteSpace(TBoxDiscount.Text) ? 0 : double.Parse(TBoxDiscount.Text)),
                        Image = _mainImageData
                        
                    };
                    newToy.Id_toy = toy.Id_toy;
                    newToy.Id_warehouse = ComboWarehouse.SelectedIndex + 1;
                    int amount;
                    int.TryParse(TBoxAmount.Text, out amount);
                    newToy.Amount_of_toys_on_warehouse = amount;
                    App.Context.Toys_on_warehouse.Add(newToy);
                    App.Context.Toys.Add(toy);
                    App.Context.SaveChanges();
                    MessageBox.Show("Игрушка успешно добавлена!");
                }
                //редактирование игрушки
                else
                {
                    _currentToy.Name = TBoxTitle.Text;
                    _currentToy.Cost = (int)decimal.Parse(TBoxCost.Text);
                    _currentToy.Id_category = ComboCategory.SelectedIndex + 1;
                    _currentToy.Discount = (int)(string.IsNullOrWhiteSpace(TBoxDiscount.Text) ? 0 :  double.Parse(TBoxDiscount.Text));
                    
                    if (_mainImageData != null)
                        _currentToy.Image = _mainImageData; 
                    var currentToy = App.Context.Toys_on_warehouse.Where(p => p.Id_toy == _currentToy.Id_toy).FirstOrDefault();
                    int amount;
                    int.TryParse(TBoxAmount.Text, out amount);
                    currentToy.Amount_of_toys_on_warehouse = amount;
                    App.Context.SaveChanges();
                    MessageBox.Show("Игрушка успешно изменена!");
                }
                NavigationService.GoBack();               
            }    
        }
    }
}
