namespace WpfApp10.Models
{
    public class AdditionalOption
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public AdditionalOption(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}