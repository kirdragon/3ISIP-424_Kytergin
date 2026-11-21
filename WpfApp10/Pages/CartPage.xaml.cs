
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.Models;
using WpfApp10.Services;

namespace WpfApp10.Pages
{
    public partial class CartPage : Page
    {
        private readonly CartService _cartService;

        public CartPage()
        {
            InitializeComponent();

            _cartService = new CartService();

            LoadCart();
        }

        private void LoadCart()
        {
            var items = _cartService.GetCart();

            CartGrid.ItemsSource = items;

            decimal total = items.Sum(item => item.Sum);

            TotalText.Text = "Итого: " + total + " руб.";
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as CartItem;

            _cartService.ChangeQuantity(item.Id, item.Quantity + 1);

            LoadCart();
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as CartItem;

            _cartService.ChangeQuantity(item.Id, item.Quantity - 1);

            LoadCart();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as CartItem;

            _cartService.RemoveFromCart(item.Id);

            LoadCart();
        }

        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {
            _cartService.ClearCart();

            LoadCart();

            MessageBox.Show("Корзина очищена.");
        }
    }
}
