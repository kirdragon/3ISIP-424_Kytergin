namespace WpfApp10.Models
{
    public class Car
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }

        public Car(string name, decimal basePrice)
        {
            Name = name;
            BasePrice = basePrice;
        }
    }
}