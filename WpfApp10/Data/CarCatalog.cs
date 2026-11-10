using System.Collections.Generic;
using WpfApp10.Models;

namespace WpfApp10.Data
{
    public class CarCatalog
    {
        public static List<Car> GetCars()
        {
            return new List<Car>
            {
                new Car("BMW 3 Series", 3500000, new List<Engine>
                {
                    new Engine("Бензин 2.0", 0),
                    new Engine("Бензин 3.0", 350000),
                    new Engine("Дизель 2.0", 150000)
                }),

                new Car("BMW 5 Series", 4800000, new List<Engine>
                {
                    new Engine("Бензин 2.0", 0),
                    new Engine("Бензин 3.0", 450000),
                    new Engine("Дизель 3.0", 300000)
                }),

                new Car("BMW X5", 6500000, new List<Engine>
                {
                    new Engine("Бензин 3.0", 0),
                    new Engine("Бензин 4.4", 800000),
                    new Engine("Дизель 3.0", 400000)
                })
            };
        }

        public static List<AdditionalOption> GetOptions()
        {
            return new List<AdditionalOption>
            {
                new AdditionalOption("Подогрев сидений", 30000),
                new AdditionalOption("Камера заднего вида", 45000),
                new AdditionalOption("Панорамная крыша", 100000)
            };
        }
    }
}