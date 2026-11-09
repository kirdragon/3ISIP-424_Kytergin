using System.Collections.Generic;

namespace WpfApp10.Models
{
    public class Car
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public List<Engine> Engines { get; set; }

        public Car(string name, decimal basePrice, List<Engine> engines)
        {
            Name = name;
            BasePrice = basePrice;
            Engines = engines;
        }
    }
}