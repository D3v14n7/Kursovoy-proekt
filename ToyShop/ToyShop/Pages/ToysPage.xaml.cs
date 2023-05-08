using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using ToyShop.Entities;
using FontFamily = System.Windows.Media.FontFamily;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы игрушек
    /// </summary>
    public partial class ToysPage : Page
    {
        /// <summary>
        /// Отображение значений по умолчанию при входе на страницу
        /// </summary>
        public ToysPage()
        {
            InitializeComponent();

            if (App.CurrentUser.RoleId == 3)//если зашел "пользователь" - отображение кнопки добавления игрушки скрыто
            {
                BtnAddToy.Visibility = Visibility.Collapsed;
            }
            else//если зашел не "пользователь" - кнопка добавления игрушки видна
            {
                BtnAddToy.Visibility = Visibility.Visible;
            }

            LViewToys.ItemsSource = App.Context.Toys.ToList();
            ComboCategory.SelectedIndex = 0;//значение выборки категории - по умолчанию "все" 
            ComboOrderBy.SelectedIndex = 0;//значение сортировки по стоимости - по умолчанию "без порядка"
            UpdateToys();

        }
        /// <summary>
        /// Переход на страницу добавления игрушки
        /// </summary>
        private void BtnAddToy_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage());
        }
        /// <summary>
        /// Переход на страницу изменения конкретной игрушки
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentToy = (sender as Button).DataContext as Entities.Toy;
            NavigationService.Navigate(new AddEditPage(currentToy));
        }
        /// <summary>
        /// Обработчик события удаления конкртеной игрушки
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentToy = (sender as Button).DataContext as Entities.Toy;
            var currentToyOnWarehouse = App.Context.Toys.Where(p => p.Id_toy == currentToy.Id_toy).FirstOrDefault();
            var amountOnWarehouse = App.Context.Toys_on_warehouse.Where(p => p.Id_toy == currentToyOnWarehouse.Id_toy).ToList();
            Cart toyInCartFromDB = App.Context.Carts.Where(p => p.Id_toy == currentToy.Id_toy && p.Id_user == App.CurrentUser.Id_user).FirstOrDefault();


            if (MessageBox.Show($"Вы уверены, что хотите удалить игрушку: " + $"{currentToy.Name}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.Context.Toys.Remove(currentToy);//удаление игрушки
                foreach (var toy in amountOnWarehouse)//удаление игрушки со склада
                {
                    App.Context.Toys_on_warehouse.Remove(toy);
                }
                App.Context.SaveChanges();
                if (App.Context.Carts.Count() == 0)
                {
                    App.Context.Purchases.Remove(App.Context.Purchases.Where(p => p.Id_purchase == toyInCartFromDB.Id_purchase).FirstOrDefault());
                }
                App.Context.SaveChanges();
                UpdateToys();
            }
        }
        /// <summary>
        /// При выборе элемента сортировки вызывается метод отображения игрушек
        /// </summary>
        private void ComboOrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateToys();
        }
        /// <summary>
        /// При выборе элемента выборки вызывается метод отображения игрушек
        /// </summary>
        private void ComboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateToys();
        }
        /// <summary>
        /// При введение в поиск вызывается метод отображения игрушек
        /// </summary>
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateToys();
        }
        /// <summary>
        /// Метод отображения игрушек
        /// </summary>
        private void UpdateToys()
        {
            var toys = App.Context.Toys.ToList();
            //сортировка игрушек по стоимости
            if (ComboOrderBy.SelectedIndex == 1)
                toys = toys.OrderBy(p => p.CostWithDiscount).ToList();
            if (ComboOrderBy.SelectedIndex == 2)
                toys = toys.OrderByDescending(p => p.CostWithDiscount).ToList();
            //выборка игрушек по категории
            if (ComboCategory.SelectedIndex == 1)
                toys = toys.Where(p => p.Id_category == 1).ToList();
            if (ComboCategory.SelectedIndex == 2)
                toys = toys.Where(p => p.Id_category == 2).ToList();
            if (ComboCategory.SelectedIndex == 3)
                toys = toys.Where(p => p.Id_category == 3).ToList();
            if (ComboCategory.SelectedIndex == 4)
                toys = toys.Where(p => p.Id_category == 4).ToList();
            if (ComboCategory.SelectedIndex == 5)
                toys = toys.Where(p => p.Id_category == 5).ToList();
            if (ComboCategory.SelectedIndex == 6)
                toys = toys.Where(p => p.Id_category == 6).ToList();
            //поиск игрушек по названию
            toys = toys.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewToys.ItemsSource = toys;//вывод игрушек в зависимости от сортировки, выборки и поиска
        }
        /// <summary>
        /// Вызов метода отображения игрушек при загрузке страницы
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateToys();
        }
        /// <summary>
        /// Переход на страницу корзины
        /// </summary>
        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CartPage());
        }
        /// <summary>
        /// Обработчик события добавления конкретной игрушки в корзину пользователя
        /// </summary>
        private void BtnToCart_Click(object sender, RoutedEventArgs e)
        {
            Purchase currentPurchase = new Purchase();
            Cart cart = new Cart();
            cart.Id_user = App.CurrentUser.Id_user;
            var purchase = App.Context.Purchases.OrderByDescending(p => p.Id_purchase).FirstOrDefault();
            var toyInCart = (sender as Button).DataContext as Entities.Toy;
            Cart toyInCartFromDB = App.Context.Carts.Where(p => p.Id_toy == toyInCart.Id_toy && p.Id_user == App.CurrentUser.Id_user).FirstOrDefault();
            var toyOnWarehouseFromDB = App.Context.Toys_on_warehouse.Where(p => p.Id_toy == toyInCart.Id_toy).FirstOrDefault();

            if (toyOnWarehouseFromDB.Amount_of_toys_on_warehouse == 0 || toyOnWarehouseFromDB.Amount_of_toys_on_warehouse == null)//если количество игрушки в наличии равно 0
            {
                MessageBox.Show("Данной игрушки нет в наличии, но вы можете выбрать другую!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (toyInCartFromDB != null)//если игрушка уже была добавлена в корзину пользователя
                {
                    MessageBox.Show("Игрушка уже есть в корзине", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else//добавление игрушки в корзину пользователя
                {
                    var carts = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).ToList();
                    var UserCart = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).FirstOrDefault();
                    if (carts.Count() != 0)
                    {
                        var idPurchase = App.Context.Purchases.Where(p => p.Id_purchase == UserCart.Id_purchase).FirstOrDefault();
                        cart.Id_purchase = idPurchase.Id_purchase;
                        cart.Id_toy = toyInCart.Id_toy;
                        cart.Cost = toyInCart.Cost - toyInCart.Discount;
                        cart.Amount_of_toys = 1;
                        App.Context.Carts.Add(cart);
                        App.Context.SaveChanges();
                        MessageBox.Show("Игрушка добавлена в корзину", "Внимание!", MessageBoxButton.OK);
                    }
                    else
                    {
                        currentPurchase.Id_purchase = purchase.Id_purchase + 1;
                        cart.Id_purchase = currentPurchase.Id_purchase;
                        cart.Id_toy = toyInCart.Id_toy;
                        cart.Cost = toyInCart.Cost - toyInCart.Discount;
                        cart.Amount_of_toys = 1;
                        App.Context.Purchases.Add(currentPurchase);
                        App.Context.Carts.Add(cart);
                        App.Context.SaveChanges();
                        MessageBox.Show("Игрушка добавлена в корзину", "Внимание!", MessageBoxButton.OK);
                    }

                }
            }

            UpdateToys();
        }
        /// <summary>
        /// Переход на страницу корзины
        /// </summary>
        private void BtnCart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new CartPage());
            UpdateToys();
        }
        /// <summary>
        /// Обработчик события кнопки печати списка игрушек и их ифнормации
        /// </summary>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument list = new FlowDocument();
            list.FontFamily = new FontFamily("Times New Roman");
            list.FontSize = 14;
            list.ColumnWidth = 1000;
            list.Blocks.Add(new Paragraph(new Run("Список игрушек\v")));
            foreach (var item in LViewToys.Items)
            {
                list.Blocks.Add(new Paragraph(new Run((item as Toy).ToPDF())));
            }
            PrintDialog print = new PrintDialog();
            if (print.ShowDialog() != true) return;
            list.PageHeight = print.PrintableAreaHeight;
            list.PageWidth = print.PrintableAreaWidth;
            IDocumentPaginatorSource idocument = list as IDocumentPaginatorSource;

            print.PrintDocument(idocument.DocumentPaginator, "Печать");
        }           
    }
}
