
using System.Collections.Generic;
using System.Linq;

namespace _3ISIP_424_Kytergin.Services
{
    public class ProductService
    {
        // Получение списка активных товаров
        public static List<Product> GetAllProducts()
        {
            return Core.Context.Product
                .Where(p => p.IsActive && p.StockQuantity > 0)
                .OrderBy(p => p.Name)
                .ToList();
        }

        // Получение товара по его идентификатору
        public static Product GetProductById(int productId)
        {
            return Core.Context.Product
                .FirstOrDefault(p => p.Id == productId && p.IsActive);
        }
    }
}
