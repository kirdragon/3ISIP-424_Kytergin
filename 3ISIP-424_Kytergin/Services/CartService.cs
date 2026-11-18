
using System.Collections.Generic;
using System.Linq;

namespace _3ISIP_424_Kytergin.Services
{
    public class CartService
    {
        // Получение корзины пользователя
        public static List<CartItem> GetCartItems(int userId)
        {
            Cart cart = Core.Context.Cart
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
                return new List<CartItem>();

            return Core.Context.CartItem
                .Include("Product")
                .Where(ci => ci.CartId == cart.Id)
                .ToList();
        }

        // Добавление товара в корзину
        public static bool AddToCart(
            int userId,
            int productId,
            int quantity,
            out string message)
        {
            if (quantity <= 0)
            {
                message = "Количество должно быть больше нуля.";
                return false;
            }

            Product product = Core.Context.Product
                .FirstOrDefault(p => p.Id == productId && p.IsActive);

            if (product == null)
            {
                message = "Товар не найден или неактивен.";
                return false;
            }

            Cart cart = Core.Context.Cart
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                Core.Context.Cart.Add(cart);
                Core.Context.SaveChanges();
            }

            CartItem cartItem = Core.Context.CartItem
                .FirstOrDefault(ci =>
                    ci.CartId == cart.Id &&
                    ci.ProductId == productId);

            int currentQuantity = cartItem == null
                ? 0
                : cartItem.Quantity;

            if (currentQuantity + quantity > product.StockQuantity)
            {
                message = "Недостаточно товара на складе.";
                return false;
            }

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };

                Core.Context.CartItem.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            Core.Context.SaveChanges();

            message = "Товар добавлен в корзину.";
            return true;
        }

        // Удаление товара из корзины
        public static bool RemoveFromCart(int userId, int cartItemId)
        {
            Cart cart = Core.Context.Cart
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
                return false;

            CartItem cartItem = Core.Context.CartItem
                .FirstOrDefault(ci =>
                    ci.Id == cartItemId &&
                    ci.CartId == cart.Id);

            if (cartItem == null)
                return false;

            Core.Context.CartItem.Remove(cartItem);
            Core.Context.SaveChanges();

            return true;
        }

        // Очистка корзины
        public static void ClearCart(int userId)
        {
            List<CartItem> items = GetCartItems(userId);

            Core.Context.CartItem.RemoveRange(items);
            Core.Context.SaveChanges();
        }
    }
}
