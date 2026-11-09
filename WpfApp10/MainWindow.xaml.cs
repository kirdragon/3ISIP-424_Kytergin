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

        private ComboBox _carComboBox;
        private ComboBox _engineComboBox;

        public MainWindow()
        {
            InitializeComponent();

            _configuration = new CarConfiguration();
            _cars = CarCatalog.GetCars();

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
    }
}