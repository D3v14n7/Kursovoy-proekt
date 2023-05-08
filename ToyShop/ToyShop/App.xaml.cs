using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ToyShop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.Toys_shopEntities Context
        { get; } = new Entities.Toys_shopEntities();

        public static Entities.User CurrentUser = null;
    }
}
