using System.Collections.Generic;
using System.Windows.Controls;
using WpfApp10.Models;

namespace WpfApp10.Pages
{
    public partial class PricePage : Page
    {
        private CarConfiguration _configuration;

        public PricePage(CarConfiguration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            CalculatePrice();
        }

        private void CalculatePrice()
        {
            decimal carPrice =
                _configuration.SelectedCar.BasePrice;

            decimal enginePrice =
                _configuration.SelectedEngine.AdditionalPrice;

            decimal colorPrice =
                _configuration.ColorAdditionalPrice;

            decimal optionsPrice = 0;

            foreach (AdditionalOption option
                in _configuration.SelectedOptions)
            {
                optionsPrice += option.Price;
            }

            decimal totalPrice =
                carPrice + enginePrice + colorPrice + optionsPrice;

            CarText.Text =
                "Автомобиль: " +
                _configuration.SelectedCar.Name +
                " — " + carPrice.ToString("N0") + " ₽";

            EngineText.Text =
                "Двигатель: " +
                _configuration.SelectedEngine.Name +
                " — " + enginePrice.ToString("N0") + " ₽";

            ColorText.Text =
                "Доплата за цвет: " +
                colorPrice.ToString("N0") + " ₽";

            OptionsText.Text =
                "Дополнительные опции: " +
                optionsPrice.ToString("N0") + " ₽";

            TotalPriceText.Text =
                "Итого: " + totalPrice.ToString("N0") + " ₽";
        }
    }
}