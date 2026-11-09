namespace WpfApp10.Models
{
    public class Engine
    {
        public string Name { get; set; }
        public decimal AdditionalPrice { get; set; }

        public Engine(string name, decimal additionalPrice)
        {
            Name = name;
            AdditionalPrice = additionalPrice;
        }
    }
}