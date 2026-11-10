using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.Data;
using WpfApp10.Models;

namespace WpfApp10
{
    public partial class MainWindow : Window
    {
        private CarConfiguration _configuration;
        private List<Car> _cars;
        private List<AdditionalOption> _options;

        private ComboBox _carComboBox;
        private ComboBox _engineComboBox;
        private ComboBox _colorComboBox;

        private Slider _downPaymentSlider;
        private ComboBox _loanMonthsComboBox;

        private TextBlock _downPaymentText;
        private TextBlock _loanAmountText;
        private TextBlock _monthlyPaymentText;

        private TextBox _nameTextBox;
        private TextBox _phoneTextBox;
        private TextBox _emailTextBox;
        private Button _submitButton;

        private int _currentStep = 1;

        public MainWindow()
        {
            InitializeComponent();

            _configuration = new CarConfiguration();
            _cars = CarCatalog.GetCars();
            _options = CarCatalog.GetOptions();

            BackButton.Click += BackButton_Click;
            NextButton.Click += NextButton_Click;

            ShowCurrentStep();
        }

        private void ShowCurrentStep()
        {
            StepTitle.Text = "Шаг " + _currentStep + " из 5";
            WizardProgress.Value = _currentStep;

            BackButton.IsEnabled = _currentStep > 1;
            NextButton.Visibility = _currentStep == 5
                ? Visibility.Collapsed
                : Visibility.Visible;

            if (_currentStep == 1)
                ShowCarSelection();
            else if (_currentStep == 2)
                ShowOptions();
            else if (_currentStep == 3)
                ShowPriceSummary();
            else if (_currentStep == 4)
                ShowLoanCalculator();
            else if (_currentStep == 5)
                ShowContactForm();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStep == 1)
            {
                if (_configuration.SelectedCar == null ||
                    _configuration.SelectedEngine == null)
                {
                    MessageBox.Show("Выберите автомобиль и двигатель.");
                    return;
                }
            }

            if (_currentStep < 5)
            {
                _currentStep++;
                ShowCurrentStep();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStep == 5 && !IsContactFormValid())
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы не заполнили все поля корректно. Покинуть страницу заявки?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No)
                    return;
            }

            if (_currentStep > 1)
            {
                _currentStep--;
                ShowCurrentStep();
            }
        }

        private void ShowCarSelection()
        {
            StepContent.Children.Clear();

            Engine savedEngine = _configuration.SelectedEngine;

            TextBlock carLabel = new TextBlock();
            carLabel.Text = "Выберите автомобиль:";
            carLabel.Margin = new Thickness(5);

            _carComboBox = new ComboBox();
            _carComboBox.ItemsSource = _cars;
            _carComboBox.DisplayMemberPath = "Name";
            _carComboBox.SelectionChanged += CarComboBox_SelectionChanged;

            TextBlock engineLabel = new TextBlock();
            engineLabel.Text = "Выберите двигатель:";
            engineLabel.Margin = new Thickness(5, 20, 5, 5);

            _engineComboBox = new ComboBox();
            _engineComboBox.SelectionChanged += EngineComboBox_SelectionChanged;

            StepContent.Children.Add(carLabel);
            StepContent.Children.Add(_carComboBox);
            StepContent.Children.Add(engineLabel);
            StepContent.Children.Add(_engineComboBox);

            int carIndex = _configuration.SelectedCar == null
                ? 0
                : _cars.IndexOf(_configuration.SelectedCar);

            _carComboBox.SelectedIndex = carIndex;

            if (savedEngine != null &&
                _configuration.SelectedCar.Engines.Contains(savedEngine))
            {
                _engineComboBox.SelectedItem = savedEngine;
            }
        }

        private void CarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Car selectedCar = _carComboBox.SelectedItem as Car;

            if (selectedCar == null)
                return;

            _configuration.SelectedCar = selectedCar;
            _configuration.SelectedEngine = null;

            _engineComboBox.ItemsSource = selectedCar.Engines;
            _engineComboBox.DisplayMemberPath = "Name";
            _engineComboBox.SelectedIndex = 0;
        }

        private void EngineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Engine selectedEngine = _engineComboBox.SelectedItem as Engine;

            if (selectedEngine == null)
                return;

            _configuration.SelectedEngine = selectedEngine;
        }

        private void ShowOptions()
        {
            StepContent.Children.Clear();

            TextBlock colorLabel = new TextBlock();
            colorLabel.Text = "Выберите цвет автомобиля:";
            colorLabel.Margin = new Thickness(5);

            _colorComboBox = new ComboBox();

            Dictionary<string, decimal> colors = ColorCatalog.GetColors();

            foreach (KeyValuePair<string, decimal> color in colors)
            {
                _colorComboBox.Items.Add(color.Key);
            }

            _colorComboBox.SelectedItem = _configuration.ColorName;
            _colorComboBox.SelectionChanged += ColorComboBox_SelectionChanged;

            TextBlock optionsLabel = new TextBlock();
            optionsLabel.Text = "Дополнительные опции:";
            optionsLabel.Margin = new Thickness(5, 25, 5, 10);

            StepContent.Children.Add(colorLabel);
            StepContent.Children.Add(_colorComboBox);
            StepContent.Children.Add(optionsLabel);

            foreach (AdditionalOption option in _options)
            {
                CheckBox checkBox = new CheckBox();

                checkBox.Content = option.Name + " — " +
                    option.Price.ToString("N0") + " руб.";

                checkBox.Margin = new Thickness(5, 8, 5, 8);
                checkBox.IsChecked = _configuration.SelectedOptions.Contains(option);

                checkBox.Checked += delegate
                {
                    if (!_configuration.SelectedOptions.Contains(option))
                        _configuration.SelectedOptions.Add(option);
                };

                checkBox.Unchecked += delegate
                {
                    _configuration.SelectedOptions.Remove(option);
                };

                StepContent.Children.Add(checkBox);
            }
        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_colorComboBox.SelectedItem == null)
                return;

            string selectedColor = _colorComboBox.SelectedItem.ToString();

            _configuration.ColorName = selectedColor;
            _configuration.ColorAdditionalPrice =
                ColorCatalog.GetColors()[selectedColor];
        }

        private decimal CalculateTotalPrice()
        {
            decimal totalPrice = 0;

            totalPrice += _configuration.SelectedCar.BasePrice;
            totalPrice += _configuration.SelectedEngine.AdditionalPrice;
            totalPrice += _configuration.ColorAdditionalPrice;

            foreach (AdditionalOption option in _configuration.SelectedOptions)
            {
                totalPrice += option.Price;
            }

            return totalPrice;
        }

        private void ShowPriceSummary()
        {
            StepContent.Children.Clear();

            decimal basePrice = _configuration.SelectedCar.BasePrice;
            decimal enginePrice = _configuration.SelectedEngine.AdditionalPrice;
            decimal colorPrice = _configuration.ColorAdditionalPrice;

            decimal optionsPrice = 0;

            foreach (AdditionalOption option in _configuration.SelectedOptions)
            {
                optionsPrice += option.Price;
            }

            TextBlock carText = new TextBlock();
            carText.Text = "Автомобиль: " + _configuration.SelectedCar.Name;
            carText.Margin = new Thickness(5);

            TextBlock engineText = new TextBlock();
            engineText.Text = "Двигатель: " + _configuration.SelectedEngine.Name;
            engineText.Margin = new Thickness(5);

            TextBlock colorText = new TextBlock();
            colorText.Text = "Цвет: " + _configuration.ColorName;
            colorText.Margin = new Thickness(5);

            TextBlock basePriceText = new TextBlock();
            basePriceText.Text = "Базовая стоимость: " +
                basePrice.ToString("N0") + " руб.";
            basePriceText.Margin = new Thickness(5, 20, 5, 5);

            TextBlock enginePriceText = new TextBlock();
            enginePriceText.Text = "Доплата за двигатель: " +
                enginePrice.ToString("N0") + " руб.";
            enginePriceText.Margin = new Thickness(5);

            TextBlock colorPriceText = new TextBlock();
            colorPriceText.Text = "Доплата за цвет: " +
                colorPrice.ToString("N0") + " руб.";
            colorPriceText.Margin = new Thickness(5);

            TextBlock optionsPriceText = new TextBlock();
            optionsPriceText.Text = "Дополнительные опции: " +
                optionsPrice.ToString("N0") + " руб.";
            optionsPriceText.Margin = new Thickness(5);

            TextBlock totalPriceText = new TextBlock();
            totalPriceText.Text = "ИТОГО: " +
                CalculateTotalPrice().ToString("N0") + " руб.";
            totalPriceText.FontSize = 20;
            totalPriceText.FontWeight = FontWeights.Bold;
            totalPriceText.Margin = new Thickness(5, 25, 5, 5);

            StepContent.Children.Add(carText);
            StepContent.Children.Add(engineText);
            StepContent.Children.Add(colorText);
            StepContent.Children.Add(basePriceText);
            StepContent.Children.Add(enginePriceText);
            StepContent.Children.Add(colorPriceText);
            StepContent.Children.Add(optionsPriceText);
            StepContent.Children.Add(totalPriceText);
        }

        private void ShowLoanCalculator()
        {
            StepContent.Children.Clear();

            TextBlock priceText = new TextBlock();
            priceText.Text = "Стоимость автомобиля: " +
                CalculateTotalPrice().ToString("N0") + " руб.";
            priceText.Margin = new Thickness(5, 5, 5, 20);

            TextBlock downPaymentLabel = new TextBlock();
            downPaymentLabel.Text = "Первоначальный взнос:";
            downPaymentLabel.Margin = new Thickness(5);

            _downPaymentSlider = new Slider();
            _downPaymentSlider.Minimum = 10;
            _downPaymentSlider.Maximum = 80;
            _downPaymentSlider.Value = (double)_configuration.DownPaymentPercent;
            _downPaymentSlider.TickFrequency = 5;
            _downPaymentSlider.IsSnapToTickEnabled = true;
            _downPaymentSlider.ValueChanged += DownPaymentSlider_ValueChanged;

            _downPaymentText = new TextBlock();
            _downPaymentText.Margin = new Thickness(5, 5, 5, 20);

            TextBlock monthsLabel = new TextBlock();
            monthsLabel.Text = "Срок кредита:";
            monthsLabel.Margin = new Thickness(5);

            _loanMonthsComboBox = new ComboBox();

            _loanMonthsComboBox.Items.Add(12);
            _loanMonthsComboBox.Items.Add(24);
            _loanMonthsComboBox.Items.Add(36);
            _loanMonthsComboBox.Items.Add(48);
            _loanMonthsComboBox.Items.Add(60);
            _loanMonthsComboBox.Items.Add(72);
            _loanMonthsComboBox.Items.Add(84);
            _loanMonthsComboBox.Items.Add(96);

            _loanMonthsComboBox.SelectedItem = _configuration.LoanMonths;
            _loanMonthsComboBox.SelectionChanged += LoanMonthsComboBox_SelectionChanged;

            _loanAmountText = new TextBlock();
            _loanAmountText.Margin = new Thickness(5, 20, 5, 5);

            _monthlyPaymentText = new TextBlock();
            _monthlyPaymentText.FontSize = 18;
            _monthlyPaymentText.FontWeight = FontWeights.Bold;
            _monthlyPaymentText.Margin = new Thickness(5, 15, 5, 5);

            StepContent.Children.Add(priceText);
            StepContent.Children.Add(downPaymentLabel);
            StepContent.Children.Add(_downPaymentSlider);
            StepContent.Children.Add(_downPaymentText);
            StepContent.Children.Add(monthsLabel);
            StepContent.Children.Add(_loanMonthsComboBox);
            StepContent.Children.Add(_loanAmountText);
            StepContent.Children.Add(_monthlyPaymentText);

            UpdateLoanCalculation();
        }

        private void DownPaymentSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_configuration == null)
                return;

            _configuration.DownPaymentPercent =
                (decimal)_downPaymentSlider.Value;

            UpdateLoanCalculation();
        }

        private void LoanMonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loanMonthsComboBox.SelectedItem == null)
                return;

            _configuration.LoanMonths =
                (int)_loanMonthsComboBox.SelectedItem;

            UpdateLoanCalculation();
        }

        private void UpdateLoanCalculation()
        {
            decimal totalPrice = CalculateTotalPrice();

            decimal downPayment = totalPrice *
                _configuration.DownPaymentPercent / 100;

            decimal loanAmount = totalPrice - downPayment;

            decimal annualRate = 12;
            double monthlyRate = (double)(annualRate / 100 / 12);

            int months = _configuration.LoanMonths;

            double rateFactor = Math.Pow(1 + monthlyRate, months);

            decimal monthlyPayment = loanAmount *
                (decimal)(monthlyRate * rateFactor / (rateFactor - 1));

            _downPaymentText.Text =
                "Сумма первоначального взноса: " +
                downPayment.ToString("N0") + " руб. (" +
                _configuration.DownPaymentPercent.ToString("N0") + "%)";

            _loanAmountText.Text =
                "Сумма кредита: " + loanAmount.ToString("N0") + " руб.";

            _monthlyPaymentText.Text =
                "Ежемесячный платёж: " +
                monthlyPayment.ToString("N0") + " руб.";
        }

        private void ShowContactForm()
        {
            StepContent.Children.Clear();

            TextBlock nameLabel = new TextBlock();
            nameLabel.Text = "Ваше имя:";
            nameLabel.Margin = new Thickness(5);

            _nameTextBox = new TextBox();
            _nameTextBox.Margin = new Thickness(5);
            _nameTextBox.Text = _configuration.CustomerName;
            _nameTextBox.TextChanged += ContactFields_TextChanged;

            TextBlock phoneLabel = new TextBlock();
            phoneLabel.Text = "Номер телефона (только цифры):";
            phoneLabel.Margin = new Thickness(5, 15, 5, 5);

            _phoneTextBox = new TextBox();
            _phoneTextBox.Margin = new Thickness(5);
            _phoneTextBox.Text = _configuration.Phone;
            _phoneTextBox.TextChanged += ContactFields_TextChanged;

            TextBlock emailLabel = new TextBlock();
            emailLabel.Text = "Электронная почта:";
            emailLabel.Margin = new Thickness(5, 15, 5, 5);

            _emailTextBox = new TextBox();
            _emailTextBox.Margin = new Thickness(5);
            _emailTextBox.Text = _configuration.Email;
            _emailTextBox.TextChanged += ContactFields_TextChanged;

            _submitButton = new Button();
            _submitButton.Content = "Оформить заявку";
            _submitButton.Width = 160;
            _submitButton.Height = 35;
            _submitButton.Margin = new Thickness(5, 25, 5, 5);
            _submitButton.IsEnabled = false;
            _submitButton.Click += SubmitButton_Click;

            StepContent.Children.Add(nameLabel);
            StepContent.Children.Add(_nameTextBox);
            StepContent.Children.Add(phoneLabel);
            StepContent.Children.Add(_phoneTextBox);
            StepContent.Children.Add(emailLabel);
            StepContent.Children.Add(_emailTextBox);
            StepContent.Children.Add(_submitButton);

            ValidateContactForm();
        }

        private void ContactFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateContactForm();
        }

        private bool IsContactFormValid()
        {
            if (_nameTextBox == null ||
                _phoneTextBox == null ||
                _emailTextBox == null)
                return false;

            string name = _nameTextBox.Text.Trim();
            string phone = _phoneTextBox.Text;
            string email = _emailTextBox.Text.Trim();

            bool nameIsValid = name.Length >= 2;

            bool phoneIsValid = phone.Length >= 10 &&
                phone.Length <= 15 &&
                Regex.IsMatch(phone, @"^\d+$");

            bool emailIsValid = Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );

            return nameIsValid && phoneIsValid && emailIsValid;
        }

        private void ValidateContactForm()
        {
            if (_submitButton == null)
                return;

            _submitButton.IsEnabled = IsContactFormValid();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsContactFormValid())
                return;

            _configuration.CustomerName = _nameTextBox.Text.Trim();
            _configuration.Phone = _phoneTextBox.Text;
            _configuration.Email = _emailTextBox.Text.Trim();

            MessageBox.Show(
                "Заявка успешно оформлена!\n\n" +
                "Имя: " + _configuration.CustomerName + "\n" +
                "Телефон: " + _configuration.Phone + "\n" +
                "Email: " + _configuration.Email + "\n\n" +
                "Стоимость автомобиля: " +
                CalculateTotalPrice().ToString("N0") + " руб.",
                "Оформление заявки",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}