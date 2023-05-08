using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToyShop.Entities
{
    public partial class Toy 
    {
        public string DiscountText
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return "";
                else
                    return $"* скидка {Discount} руб."; 
            }
        }
        public string TotalCost
        {
            get
            {
                var AmountWarehouse = new List<Toys_on_warehouse>();
                int? index = 0;
                foreach (var toy_in_warehouse in Toys_on_warehouse)
                {
                    index += toy_in_warehouse.Amount_of_toys_on_warehouse;
                }

                    if (Discount == 0 || Discount == null || index == 0)
                {
                    return $"{Cost:N2} рублей";
                }
                else
                {
                    return $" {CostWithDiscount:N2} рублей";
                }    
            }
        }
        public double CostWithDiscount
        {
            get
            {
                if (Discount == 0 || Discount == null)
                {
                    return (double)Cost;
                }
                else
                {
                    var costWithDiscount = (double)Cost - Discount;
                    return costWithDiscount.Value;
                }
            }
        }
        public Visibility DiscountVisible
        {
            get
            {
                var AmountWarehouse = new List<Toys_on_warehouse>();
                int? index = 0;
                foreach (var toy_in_warehouse in Toys_on_warehouse)
                {
                    index += toy_in_warehouse.Amount_of_toys_on_warehouse;
                }


                if (Discount == 0 || Discount == null || index == 0)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        public string BackColor
        {
            get
            {
                var AmountWarehouse = new List<Toys_on_warehouse>();
                int? index = 0;
                foreach (var toy_in_warehouse in Toys_on_warehouse)
                {
                    index += toy_in_warehouse.Amount_of_toys_on_warehouse;
                }


                if (index == 0)
                {
                    return "#FF9999";
                }
                else
                {
                    if (Discount == 0 || Discount == null)
                        return "#EFC571";
                    else
                        return "#D1FFD1";
                }
            }
        }
        public string AdminControlsVisibility
        {
            get
            {
                if (App.CurrentUser.RoleId == 1)
                {
                    return "Visible";
                }
                if (App.CurrentUser.RoleId == 2)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
        }
        public string NameCategory
        {
            get
            {
                return $"{Toys_category.Name_category}";
            }
        }
        public string AmountText
        {
            get 
            {
                var AmountWarehouse = new List<Toys_on_warehouse>();
                int? index = 0;
                foreach (var toy_in_warehouse in Toys_on_warehouse)
                {
                    index += toy_in_warehouse.Amount_of_toys_on_warehouse;
                }
                if (index > 0)
                    return $"В наличии: {index.ToString()}";
                else
                    return $"Нет в наличии";
            }
        }
        public string ToPDF()
        {
            return $"Название: {Name}; цена: {TotalCost}\nКатегория: {NameCategory} \n{AmountText}";
        }
    }
}
