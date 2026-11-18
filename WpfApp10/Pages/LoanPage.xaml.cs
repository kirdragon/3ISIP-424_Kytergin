
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.Models;

namespace WpfApp10.Pages
{
    public partial class LoanPage : Page
    {
        private CarConfiguration _configuration;
        private bool _isInitializing = true;

        public LoanPage(CarConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            LoanMonthsComboBox.Items.Add(12);
            LoanMonthsComboBox.Items.Add(24);
            LoanMonthsComboBox.Items.Add(36);
            LoanMonthsComboBox.Items.Add(48);
            LoanMonthsComboBox.Items.Add(60);

            DownPaymentSlider.Value =
                (double)_configuration.DownPaymentPercent;

            LoanMonthsComboBox.SelectedItem =
                _configuration.LoanMonths;

            _isInitializing = false;

            DownPaymentSlider.ValueChanged +=
                DownPaymentSlider_ValueChanged;

            LoanMonthsComboBox.SelectionChanged +=
                LoanMonthsComboBox_SelectionChanged;

            UpdateLoanCalculation();
        }

        private void DownPaymentSlider_ValueChanged(
            object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isInitializing)
                return;

            UpdateLoanCalculation();
        }

        private void LoanMonthsComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (_isInitializing)
                return;

            UpdateLoanCalculation();
        }

        private void UpdateLoanCalculation()
        {
            if (_configuration.SelectedCar == null ||
                _configuration.SelectedEngine == null ||
                LoanMonthsComboBox.SelectedItem == null)
            {
                return;
            }

            decimal totalPrice =
                _configuration.SelectedCar.BasePrice +
                _configuration.SelectedEngine.AdditionalPrice +
                _configuration.ColorAdditionalPrice;

            foreach (AdditionalOption option
                in _configuration.SelectedOptions)
            {
                totalPrice += option.Price;
            }

            decimal downPaymentPercent =
                (decimal)DownPaymentSlider.Value;

            int months =
                (int)LoanMonthsComboBox.SelectedItem;

            decimal downPayment =
                totalPrice * downPaymentPercent / 100;

            decimal loanAmount =
                totalPrice - downPayment;

            decimal annualInterestRate = 0.12m;

            double monthlyRate =
                (double)(annualInterestRate / 12);

            double ratePower =
                Math.Pow(1 + monthlyRate, months);

            decimal monthlyPayment =
                loanAmount *
                (decimal)(
                    monthlyRate * ratePower /
                    (ratePower - 1));

            _configuration.DownPaymentPercent =
                downPaymentPercent;

            _configuration.LoanMonths = months;

            DownPaymentText.Text =
                "Первоначальный взнос: " +
                downPaymentPercent.ToString("N0") +
                "% (" + downPayment.ToString("N0") + " ₽)";

            TotalPriceText.Text =
                totalPrice.ToString("N0") + " ₽";

            LoanAmountText.Text =
                "Сумма кредита: " +
                loanAmount.ToString("N0") + " ₽";

            MonthlyPaymentText.Text =
                "Ежемесячный платёж: " +
                monthlyPayment.ToString("N0") + " ₽";
        }
    }
}
