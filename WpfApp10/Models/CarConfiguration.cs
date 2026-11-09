using System.Collections.Generic;

namespace WpfApp10.Models
{
    public class CarConfiguration
    {
        public Car SelectedCar { get; set; }
        public Engine SelectedEngine { get; set; }

        public string ColorName { get; set; }
        public decimal ColorAdditionalPrice { get; set; }

        public List<AdditionalOption> SelectedOptions { get; set; }

        public decimal DownPaymentPercent { get; set; }
        public int LoanMonths { get; set; }

        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public CarConfiguration()
        {
            SelectedOptions = new List<AdditionalOption>();

            ColorName = "Белый";
            DownPaymentPercent = 20;
            LoanMonths = 36;

            CustomerName = "";
            Phone = "";
            Email = "";
        }
    }
}