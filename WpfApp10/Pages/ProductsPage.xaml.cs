
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.Services;

namespace WpfApp10.Pages
{
    public partial class ProductsPage : Page
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;

        public ProductsPage()
        {
            InitializeComponent();

            _productService = new ProductService();
            _cartService = new CartService();

            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = _productService.GetProducts();

            foreach (var product in products)
            {
                if (!string.IsNullOrWhiteSpace(product.ImagePath))
                {
                    product.ImagePath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        product.ImagePath);
                }
            }

            ProductsGrid.ItemsSource = products;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int productId = (int)button.Tag;

            _cartService.AddToCart(productId);

            MessageBox.Show("Товар добавлен в корзину!");
        }
    }
}
