using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ToyShop.Entities
{
    public partial class Cart
    {
        public string ToyCost
        {
            get
            {
                return $"{((int)(Toy.Cost - Toy.Discount) * Amount_of_toys):N2} рублей";
            }
        }
        public string ToyText
        {
            get
            {
                return $"{Toy.Name}";
            }
        }
        public string BackColor
        {
            get
            {
               return "#D1FFD1";
            }
        }
        public byte[] ToyImage
        {
            get
            {
                 return Toy.Image;
            }
        }
    }
}
