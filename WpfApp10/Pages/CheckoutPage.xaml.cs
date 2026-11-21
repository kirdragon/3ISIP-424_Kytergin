
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.Services;

namespace WpfApp10.Pages
{
    public partial class CheckoutPage : Page
    {
        private readonly OrderService _orderService;

        public CheckoutPage()
        {
            InitializeComponent();

            _orderService = new OrderService();
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string address = AddressTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            try
            {
                _orderService.CreateOrder(fullName, email, address);

                MessageBox.Show("Заказ успешно оформлен!");

                FullNameTextBox.Clear();
                EmailTextBox.Clear();
                AddressTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка оформления заказа");
            }
        }
    }
}
