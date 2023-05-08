using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.Entities
{
    public partial class Employee
    {
        public string PositionText
        {
            get
            {
                return $"{Position.Name_position}";
            }
        }
        public string BackColor
        {
            get
            {
               return "#D1FFD1";
            }
        }
        public string PhoneEmployee
        {
            get
            {
               return $"Тел: {Phone_number}";
            }
        }
        public override string ToString()
        {
            return $"{Surname_employee} {Name_employee} {Patronymic_employee}";
        }
    }
}
