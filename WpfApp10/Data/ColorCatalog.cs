using System.Collections.Generic;

namespace WpfApp10.Data
{
    public class ColorCatalog
    {
        public static Dictionary<string, decimal> GetColors()
        {
            return new Dictionary<string, decimal>
            {
                { "Белый", 0 },
                { "Чёрный", 50000 },
                { "Красный", 70000 },
                { "Синий", 60000 },
                { "Серый", 40000 }
            };
        }
    }
}