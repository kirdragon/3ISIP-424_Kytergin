
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.Models;

namespace WpfApp10.Pages
{
    public partial class ContactPage : Page
    {
        private CarConfiguration _configuration;

        public ContactPage(CarConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            CustomerNameTextBox.Text =
                _configuration.CustomerName;

            PhoneTextBox.Text =
                _configuration.Phone;

            EmailTextBox.Text =
                _configuration.Email;

            ValidateContactForm();
        }

        private void ContactFields_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ValidateContactForm();
        }

        public bool IsContactFormValid()
        {
            string name = CustomerNameTextBox.Text.Trim();

            string phone = Regex.Replace(
                PhoneTextBox.Text,
                @"\D",
                "");

            string email = EmailTextBox.Text.Trim();

            bool validName = name.Length >= 2;

            bool validPhone =
                phone.Length >= 10 &&
                phone.Length <= 15;

            bool validEmail =
                Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return validName && validPhone && validEmail;
        }

        private void ValidateContactForm()
        {
            SubmitButton.IsEnabled = IsContactFormValid();
        }

        private void SubmitButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!IsContactFormValid())
            {
                MessageBox.Show(
                    "Проверьте правильность заполнения полей.");

                return;
            }

            _configuration.CustomerName =
                CustomerNameTextBox.Text.Trim();

            _configuration.Phone =
                PhoneTextBox.Text.Trim();

            _configuration.Email =
                EmailTextBox.Text.Trim();

            MessageBox.Show(
                "Заявка успешно оформлена!\n\n" +
                "Имя: " + _configuration.CustomerName + "\n" +
                "Телефон: " + _configuration.Phone + "\n" +
                "Email: " + _configuration.Email,
                "Заявка оформлена",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
