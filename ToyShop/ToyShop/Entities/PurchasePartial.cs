using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToyShop.Entities
{
    public partial class Purchase
    {
        public string BackColor
        {
            get
            {
                return "#D1FFD1";
            }
        }
        public string BuyerName
        {
            get
            {
                return $"Покупатель: {Buyer.Surname_buyer} {Buyer.Name_buyer} {Buyer.Patronymic_buyer}";
            }
        }
        public string PhoneBuyer
        {
            get
            {
                return $"Тел: {Buyer.Phone_number}";
            }
        }
        public string EmployeeName
        {
            get
            {
                if (Id_employee == 0 || Id_employee == null)
                {
                    return $"";
                }
                else
                {
                    return $"Сотрудник: {Employee.Surname_employee} {Employee.Name_employee} {Employee.Patronymic_employee}";
                }
            }
        }
        public string DatePurchase
        {
            get
            {
                return $"Дата: " + Convert.ToDateTime(Date_purchase).ToString("dd MMMM yyyy");
            }
        }
        public string CostText
        {
            get
            {
                return $"Стоимость: {Cost:N2} рублей";
            }
        }
    }
}
