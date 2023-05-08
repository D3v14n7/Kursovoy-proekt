using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы покупок
    /// </summary>
    public partial class PurchasePage : Page
    {
        public PurchasePage()
        {
            InitializeComponent();
            LViewPurchase.ItemsSource = App.Context.Purchases.Where(p => p.Id_buyer != null).ToList();
            ComboOrderByPrice.SelectedIndex = 0;//по умолчанию нет порядка по стоимости
            ComboOrderByData.SelectedIndex = 0;//по умолчанию показывать сначала новые покупки
            UpdatePurchases();
        }
        /// <summary>
        /// При выборе выборе элемента сортировки по цене вызывается метод отображения покупок
        /// </summary>
        private void ComboSortByPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePurchases();
        }
        /// <summary>
        /// Обработчик события удаления покупки. Вместе с покупокой удаляется и клиент
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentPurchase = (sender as Button).DataContext as Entities.Purchase;
            var buyerPurchase = App.Context.Purchases.Where(p => p.Id_buyer == currentPurchase.Id_buyer).FirstOrDefault();
            var currentBuyer = App.Context.Buyers.Where(p => p.Id_buyer == buyerPurchase.Id_buyer).ToList();

            if (MessageBox.Show("Вы уверены, что хотите удалить покупку?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (var buyer in currentBuyer)
                {
                    App.Context.Buyers.Remove(buyer);
                }
                App.Context.Purchases.Remove(currentPurchase);
                App.Context.SaveChanges();
                UpdatePurchases();
            }
        }
        /// <summary>
        /// Переход на страницу добавления покупки
        /// </summary>
        private void BtnAddPurchase_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPurchasePage());
        }
        /// <summary>
        /// Метод отображения покупок
        /// </summary>
        private void UpdatePurchases()
        {
            var purchase = App.Context.Purchases.Where(p => p.Id_buyer != null).ToList();
            //сортировка по стоимости всех покупок
            if (ComboOrderByPrice.SelectedIndex == 1)
                purchase = purchase.OrderBy(p => p.Cost).ToList();
            if (ComboOrderByPrice.SelectedIndex == 2)
                purchase = purchase.OrderByDescending(p => p.Cost).ToList();
            //сортировка по дате покупки
            if (ComboOrderByData.SelectedIndex == 0)
                purchase = purchase.OrderByDescending(p => p.Date_purchase).ToList();
            if (ComboOrderByData.SelectedIndex == 1)
                purchase = purchase.OrderBy(p => p.Date_purchase).ToList();
            
            LViewPurchase.ItemsSource = purchase;//вывод покупок, в зависимости от сортировки
            
        }
        /// <summary>
        /// Вызов метода отображения покупок при загрузке страницы
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePurchases();
        }
        /// <summary>
        /// При выборе выборе элемента сортировки по дате вызывается метод отображения покупок
        /// </summary>
        private void ComboSortByData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePurchases();
        }
    }
}
