
using System.Collections.Generic;
using System.Windows.Controls;
using WpfApp10.Data;
using WpfApp10.Models;

namespace WpfApp10.Pages
{
    public partial class CarPage : Page
    {
        private CarConfiguration _configuration;
        private List<Car> _cars;
        private bool _isInitializing = true;

        public CarPage(CarConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;
            _cars = CarCatalog.GetCars();

            CarComboBox.ItemsSource = _cars;
            CarComboBox.DisplayMemberPath = "Name";

            CarComboBox.SelectionChanged += CarComboBox_SelectionChanged;
            EngineComboBox.SelectionChanged += EngineComboBox_SelectionChanged;

            Engine savedEngine = _configuration.SelectedEngine;

            if (_configuration.SelectedCar != null)
                CarComboBox.SelectedItem = _configuration.SelectedCar;
            else if (_cars.Count > 0)
                CarComboBox.SelectedIndex = 0;

            _configuration.SelectedCar =
                CarComboBox.SelectedItem as Car;

            UpdateEngines();

            if (savedEngine != null &&
                _configuration.SelectedCar != null &&
                _configuration.SelectedCar.Engines.Contains(savedEngine))
            {
                EngineComboBox.SelectedItem = savedEngine;
                _configuration.SelectedEngine = savedEngine;
            }
            else
            {
                _configuration.SelectedEngine = null;
            }

            _isInitializing = false;
        }

        private void CarComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (_isInitializing)
                return;

            _configuration.SelectedCar =
                CarComboBox.SelectedItem as Car;

            _configuration.SelectedEngine = null;

            UpdateEngines();
        }

        private void EngineComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (_isInitializing)
                return;

            _configuration.SelectedEngine =
                EngineComboBox.SelectedItem as Engine;
        }

        private void UpdateEngines()
        {
            EngineComboBox.ItemsSource = null;

            Car selectedCar = _configuration.SelectedCar;

            if (selectedCar != null)
            {
                EngineComboBox.ItemsSource = selectedCar.Engines;
                EngineComboBox.DisplayMemberPath = "Name";
            }
        }
    }
}
