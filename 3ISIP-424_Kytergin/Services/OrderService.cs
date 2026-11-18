
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3ISIP_424_Kytergin.Services
{
    public class OrderService
    {
        // Получение активных пунктов выдачи
        public static List<PickupPoint> GetPickupPoints()
        {
            return Core.Context.PickupPoint
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToList();
        }

        // Покупка всей корзины
        public static bool PurchaseCart(
            int userId,
            int pickupPointId,
            out string message)
        {
            Cart cart = Core.Context.Cart
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                message = "Корзина не найдена.";
                return false;
            }

            List<CartItem> items = Core.Context.CartItem
                .Where(ci => ci.CartId == cart.Id)
                .ToList();

            if (items.Count == 0)
            {
                message = "Корзина пуста.";
                return false;
            }

            return CreateOrder(
                userId, pickupPointId, items, out message);
        }

        // Покупка одного товара из корзины
        public static bool PurchaseCartItem(
            int userId,
            int cartItemId,
            int pickupPointId,
            out string message)
        {
            Cart cart = Core.Context.Cart
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                message = "Корзина не найдена.";
                return false;
            }

            CartItem item = Core.Context.CartItem
                .FirstOrDefault(ci =>
                    ci.Id == cartItemId &&
                    ci.CartId == cart.Id);

            if (item == null)
            {
                message = "Товар не найден в корзине.";
                return false;
            }

            List<CartItem> items = new List<CartItem>();
            items.Add(item);

            return CreateOrder(
                userId, pickupPointId, items, out message);
        }

        // Общая логика оформления заказа
        private static bool CreateOrder(
            int userId,
            int pickupPointId,
            List<CartItem> items,
            out string message)
        {
            using (var transaction =
                Core.Context.Database.BeginTransaction())
            {
                try
                {
                    // Проверяем пункт выдачи
                    PickupPoint pickupPoint =
                        Core.Context.PickupPoint.FirstOrDefault(
                            p => p.Id == pickupPointId && p.IsActive);

                    if (pickupPoint == null)
                    {
                        message = "Пункт выдачи не найден или неактивен.";
                        return false;
                    }

                    // Проверяем товары и остатки
                    foreach (CartItem item in items)
                    {
                        Product product =
                            Core.Context.Product.FirstOrDefault(
                                p => p.Id == item.ProductId);

                        if (product == null ||
                            !product.IsActive ||
                            product.StockQuantity < item.Quantity)
                        {
                            message = "Один из товаров недоступен или отсутствует в нужном количестве.";
                            return false;
                        }
                    }

                    // Создаём заказ
                    Order order = new Order
                    {
                        UserId = userId,
                        PickupPointId = pickupPointId,
                        CreatedAt = DateTime.Now,
                        Status = "Оформлен"
                    };

                    Core.Context.Order.Add(order);
                    Core.Context.SaveChanges();

                    // Добавляем товары в заказ
                    foreach (CartItem item in items)
                    {
                        Product product =
                            Core.Context.Product.First(
                                p => p.Id == item.ProductId);

                        OrderItem orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price
                        };

                        Core.Context.OrderItem.Add(orderItem);

                        // Уменьшаем остаток на складе
                        product.StockQuantity -= item.Quantity;

                        // Удаляем купленную позицию из корзины
                        Core.Context.CartItem.Remove(item);
                    }

                    Core.Context.SaveChanges();

                    transaction.Commit();

                    message = "Заказ успешно оформлен!";
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    message = "Не удалось оформить заказ. Проверьте данные и повторите попытку.";
                    return false;
                }
            }
        }

        // Получение истории заказов пользователя
        public static List<Order> GetOrderHistory(int userId)
        {
            return Core.Context.Order
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }
    }
}
