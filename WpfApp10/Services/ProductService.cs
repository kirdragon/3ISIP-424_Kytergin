
using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp10.Data;
using WpfApp10.Models;

namespace WpfApp10.Services
{
    public class ProductService
    {
        public List<Product> GetProducts()
        {
            try
            {
                using (var db = new ShopContext())
                {
                    return db.Products.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    ex.ToString(),
                    "Ошибка подключения к базе данных");

                return new List<Product>();
            }
        }
    }
}
