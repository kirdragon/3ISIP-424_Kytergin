
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp10.Data;
using WpfApp10.Models;

namespace WpfApp10.Pages
{
    public partial class OptionsPage : Page
    {
        private CarConfiguration _configuration;
        private bool _isInitializing = true;

        public OptionsPage(CarConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            Dictionary<string, decimal> colors =
                ColorCatalog.GetColors();

            ColorComboBox.ItemsSource = colors.Keys;

            if (colors.ContainsKey(_configuration.ColorName))
                ColorComboBox.SelectedItem = _configuration.ColorName;

            List<AdditionalOption> options =
                CarCatalog.GetOptions();

            OptionsPanel.Children.Clear();

            foreach (AdditionalOption option in options)
            {
                CheckBox checkBox = new CheckBox();

                checkBox.Content =
                    option.Name + " — " +
                    option.Price.ToString("N0") + " ₽";

                checkBox.Tag = option;
                checkBox.Margin = new Thickness(5, 8, 5, 8);
                checkBox.FontSize = 15;

                checkBox.IsChecked =
                    _configuration.SelectedOptions.Contains(option);

                checkBox.Checked += Option_Changed;
                checkBox.Unchecked += Option_Changed;

                OptionsPanel.Children.Add(checkBox);
            }

            ColorComboBox.SelectionChanged +=
                ColorComboBox_SelectionChanged;

            _isInitializing = false;
        }

        private void ColorComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (_isInitializing ||
                ColorComboBox.SelectedItem == null)
                return;

            _configuration.ColorName =
                ColorComboBox.SelectedItem.ToString();

            Dictionary<string, decimal> colors =
                ColorCatalog.GetColors();

            _configuration.ColorAdditionalPrice =
                colors[_configuration.ColorName];
        }

        private void Option_Changed(
            object sender,
            RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox == null)
                return;

            AdditionalOption option =
                checkBox.Tag as AdditionalOption;

            if (option == null)
                return;

            if (checkBox.IsChecked == true)
            {
                if (!_configuration.SelectedOptions.Contains(option))
                    _configuration.SelectedOptions.Add(option);
            }
            else
            {
                _configuration.SelectedOptions.Remove(option);
            }
        }
    }
}
