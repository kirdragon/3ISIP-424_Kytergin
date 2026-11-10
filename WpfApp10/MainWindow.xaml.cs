using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();

            _configuration = new CarConfiguration();
            _cars = CarCatalog.GetCars();
            _options = CarCatalog.GetOptions();

            ShowCarSelection();
        }

        private void ShowCarSelection()
        {
            StepContent.Children.Clear();

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

            _carComboBox.SelectedIndex = 0;
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

                checkBox.Content = option.Name + " — " + option.Price + " руб.";
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
            basePriceText.Text = "Базовая стоимость: " + basePrice.ToString("N0") + " руб.";
            basePriceText.Margin = new Thickness(5, 20, 5, 5);

            TextBlock enginePriceText = new TextBlock();
            enginePriceText.Text = "Доплата за двигатель: " + enginePrice.ToString("N0") + " руб.";
            enginePriceText.Margin = new Thickness(5);

            TextBlock colorPriceText = new TextBlock();
            colorPriceText.Text = "Доплата за цвет: " + colorPrice.ToString("N0") + " руб.";
            colorPriceText.Margin = new Thickness(5);

            TextBlock optionsPriceText = new TextBlock();
            optionsPriceText.Text = "Дополнительные опции: " + optionsPrice.ToString("N0") + " руб.";
            optionsPriceText.Margin = new Thickness(5);

            TextBlock totalPriceText = new TextBlock();
            totalPriceText.Text = "ИТОГО: " + CalculateTotalPrice().ToString("N0") + " руб.";
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
    }
}