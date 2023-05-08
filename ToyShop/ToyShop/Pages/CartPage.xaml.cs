using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ToyShop.Entities;
using FIOLib;
using PhoneLib;

namespace ToyShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы корзины
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            UpdateCart();
        }
        /// <summary>
        /// Метод отображения данных корзины для конкретного пользователя
        /// </summary>
        private void UpdateCart()
        {
            //отображение надписи "Ваша корзина пуста", если пользователь не добавил ни одной игрушки в корзину
            var cart = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).ToList();
            if (cart.Count == 0)
            {
                EmptyCart.Visibility = Visibility.Visible;
            }
            LViewCart.ItemsSource = cart; //отображение игрушек в корзине
            //подсчет общей стоимости игрушек в корзине, в зависимости от их количества
            var totalCost = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user);
            {
                if (totalCost != null)
                {
                    var summa = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).Sum(p => p.Cost) ?? 0;
                    TotalCost.Text = $"Итого: {summa.ToString("N2")} рублей";
                }
                Date.SelectedDate = DateTime.Today;//отображение текущей даты при оформлении заказа
            }            
        }
        /// <summary>
        /// Вызов метода отображения данных корзины при загрузке страницы
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCart();
        }
        /// <summary>
        /// Обработчик события уменьшения количества конкретной игрушки в корзине
        /// </summary>
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Cart currentToyInCart = (sender as Button).DataContext as Entities.Cart;//ссылка на конкретную игрушку в корзине
            Cart toyInCartFromDB = App.Context.Carts.Where(p => p.Id_toy == currentToyInCart.Id_toy && p.Id_user == App.CurrentUser.Id_user).FirstOrDefault();
            var toyCostInCart = App.Context.Toys.Where(p => p.Id_toy == toyInCartFromDB.Id_toy).FirstOrDefault();

            //уменьшение количества и стоимости
            if (toyInCartFromDB != null)
            {
                toyInCartFromDB.Amount_of_toys--;
                toyInCartFromDB.Cost -= toyCostInCart.Cost - toyCostInCart.Discount;
            }
            //удаление игрушки, если ее количество в корзине равно нулю
            if (toyInCartFromDB.Amount_of_toys == 0)
            {
                var warning = MessageBox.Show($"Вы уверены, что хотите удалить " + $"{currentToyInCart.Toy.Name}" + " из корзины?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //В случае соглашения - удалить игрушку из корзины
                if (warning == MessageBoxResult.Yes)
                {
                    App.Context.Carts.Remove(currentToyInCart);
                    App.Context.SaveChanges();
                    if (App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).Count() == 0)
                    {
                        App.Context.Purchases.Remove(App.Context.Purchases.Where(p => p.Id_purchase == toyInCartFromDB.Id_purchase).FirstOrDefault());
                    }
                    UpdateCart();
                }
                //В случае отказа - вернуть игрушке в корзине количество 1
                else
                {
                    toyInCartFromDB.Amount_of_toys = 1;
                    toyInCartFromDB.Cost = toyCostInCart.Cost - toyCostInCart.Discount;
                }
            }         
            App.Context.SaveChanges();
            UpdateCart();
        }
        /// <summary>
        /// Обработчик события увеличения количества конкретной игрушки в корзине
        /// </summary>
        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            Cart currentToyInCart = (sender as Button).DataContext as Entities.Cart;
            Toy toy = App.Context.Toys.Where(p => p.Id_toy == currentToyInCart.Id_toy).FirstOrDefault();
            var count = App.Context.Toys_on_warehouse.Where(p => p.Id_toy == toy.Id_toy).FirstOrDefault();
            Cart toyInCartFromDB = App.Context.Carts.Where(p => p.Id_toy == currentToyInCart.Id_toy && p.Id_user == App.CurrentUser.Id_user).FirstOrDefault();
            var toyCostInCart = App.Context.Toys.Where(p => p.Id_toy == toyInCartFromDB.Id_toy).FirstOrDefault();
            //добавление количества игрушки в зависимости от количества в наличии
            if (toyInCartFromDB.Amount_of_toys < count.Amount_of_toys_on_warehouse)
            {
                toyInCartFromDB.Amount_of_toys++;
                toyInCartFromDB.Cost += toyCostInCart.Cost - toyCostInCart.Discount;
            }
            //сообщение, если пользователь пробует добавить игрушке количество, большего количества в наличии 
            else
            {
                MessageBox.Show("Нельзя добавить больше игрушек!", "Ошибка", MessageBoxButton.OK);
            }
            App.Context.SaveChanges();
            UpdateCart();
        }
        /// <summary>
        /// Обработчик события удаления конкретной игрушки из корзины пользователя
        /// </summary>
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            Cart currentToyInCart = (sender as Button).DataContext as Entities.Cart;
            Cart toyInCartFromDB = App.Context.Carts.Where(p => p.Id_toy == currentToyInCart.Id_toy && p.Id_user == App.CurrentUser.Id_user).FirstOrDefault();

            var warning = MessageBox.Show($"Вы уверены, что хотите удалить " + $"{currentToyInCart.Toy.Name}" + " из корзины?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (warning == MessageBoxResult.Yes)
            {
                App.Context.Carts.Remove(toyInCartFromDB);                
                App.Context.SaveChanges();
                if (App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).Count() == 0)
                {
                    App.Context.Purchases.Remove(App.Context.Purchases.Where(p => p.Id_purchase == toyInCartFromDB.Id_purchase).FirstOrDefault());
                }
                UpdateCart();
                App.Context.SaveChanges();
                
            }
            
        }
        /// <summary>
        /// Обработчик события полной отчистки корзины пользователя
        /// </summary>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var carts = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).ToList();
            var purchaseCart = App.Context.Carts.OrderBy(p => p.Id_purchase).FirstOrDefault();           
            if (carts.Count() == 0)
            {
                MessageBox.Show("Из вашей корзины нечего убирать!", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var warning = MessageBox.Show("Вы уверены, что хотите очистить корзину?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (warning == MessageBoxResult.Yes)
                {
                    var currentPurchaseId = App.Context.Purchases.Where(p => p.Id_purchase == purchaseCart.Id_purchase).FirstOrDefault();
                    foreach (var cart in carts)
                    {
                        App.Context.Carts.Remove(cart);                       
                    }
                    App.Context.Purchases.Remove(currentPurchaseId);
                    
                }
            }
            App.Context.SaveChanges();
            UpdateCart();
        }
        /// <summary>
        /// Обработчик ошибок при заполнении данных для формирования покупки
        /// </summary>
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxSurname.Text)) errorBuilder.AppendLine("Фамилия должна быть заполнена;");
            else if (!FIO.NameСorrectness(TBoxSurname.Text)) errorBuilder.AppendLine("Некорректная фамилия");


            if (string.IsNullOrWhiteSpace(TBoxName.Text)) errorBuilder.AppendLine("Имя должно быть заполнено;");
            else if (!FIO.NameСorrectness(TBoxName.Text)) errorBuilder.AppendLine("Некорректное имя");

            if (string.IsNullOrWhiteSpace(TBoxPatronymic.Text)) errorBuilder.AppendLine("Отчество должно быть заполнено;");
            else if (!FIO.NameСorrectness(TBoxPatronymic.Text)) errorBuilder.AppendLine("Некорректное отчество");


            if (string.IsNullOrWhiteSpace(TBoxPhone.Text)) errorBuilder.AppendLine("Номер телефона обязателен для заполнения;");
            else if (!Phone.PhoneCorrectness(TBoxPhone.Text)) errorBuilder.AppendLine("Некорректный номер телефона");


            if (string.IsNullOrWhiteSpace(Date.Text)) errorBuilder.AppendLine("Дата обязательна для заполнения;");

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();
        }
        /// <summary>
        /// Обработчик события формирования покупки
        /// </summary>
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var carts = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).ToList();
                //проверка наполнения корзины при формировании покупки
                if (carts.Count() == 0)
                {
                    MessageBox.Show("Ваша корзина пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var toyCart = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).ToList();
                    var amountOnWarehouse = App.Context.Toys_on_warehouse.ToList();
                    for (int j = 0; j < amountOnWarehouse.Count(); j++)
                    {
                        for (int i = 0; i < toyCart.Count(); i++)
                        {
                            if (toyCart[i].Id_toy == amountOnWarehouse[j].Id_toy)
                            {
                                amountOnWarehouse[j].Amount_of_toys_on_warehouse = amountOnWarehouse[j].Amount_of_toys_on_warehouse - toyCart[i].Amount_of_toys;//уменьшение количества игрушек в зависимости от количества при покупке
                            }
                        }
                    }
                    Purchase purchase = new Purchase();
                    Buyer buyer = new Buyer();
                    var purchaseCart = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).OrderBy(p => p.Id_purchase).FirstOrDefault();
                    var currentPurchaseId = App.Context.Purchases.Where(p => p.Id_purchase == purchaseCart.Id_purchase).FirstOrDefault();
                    App.Context.Purchases.Remove(currentPurchaseId);
                    DateTime date;
                    DateTime.TryParse(Date.Text, out date);
                    purchase.Id_purchase = purchaseCart.Id_purchase;
                    purchase.Date_purchase = date;
                    purchase.Cost = App.Context.Carts.Where(p => p.Id_user == App.CurrentUser.Id_user).Sum(p => p.Cost);//внесение общей стоимости покупки
                    buyer.Name_buyer = TBoxName.Text;
                    buyer.Surname_buyer = TBoxSurname.Text;
                    buyer.Patronymic_buyer = TBoxPatronymic.Text;
                    buyer.Phone_number = TBoxPhone.Text;
                    App.Context.Buyers.Add(buyer);
                    purchase.Id_buyer = buyer.Id_buyer;
                    App.Context.Purchases.Add(purchase);
                    foreach (var cart in carts)
                    {
                        App.Context.Carts.Remove(cart);
                    }
                    App.Context.SaveChanges();
                    MessageBox.Show("Покупка сформирована!", "Внимание!", MessageBoxButton.OK);
                    UpdateCart();
                }
            }
        }           
    }
}
