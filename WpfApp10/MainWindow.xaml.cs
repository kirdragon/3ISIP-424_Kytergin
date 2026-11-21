
using System.Windows;
using WpfApp10.Pages;

namespace WpfApp10
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ProductsPage());
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CartPage());
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CheckoutPage());
        }
    }
}
