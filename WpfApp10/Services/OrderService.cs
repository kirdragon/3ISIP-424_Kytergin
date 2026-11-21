
using System;
using System.Linq;
using System.Data.Entity;
using WpfApp10.Data;
using WpfApp10.Models;

namespace WpfApp10.Services
{
    public class OrderService
    {
        public void CreateOrder(
            string fullName,
            string email,
            string address)
        {
            using (var db = new ShopContext())
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var cartItems = db.CartItems
                        .Include(c => c.Product)
                        .ToList();

                    if (!cartItems.Any())
                        throw new Exception("Корзина пуста.");

                    decimal totalPrice = cartItems.Sum(
                        c => c.Product.Price * c.Quantity);

                    var order = new Order
                    {
                        FullName = fullName,
                        Email = email,
                        Address = address,
                        OrderDate = DateTime.Now,
                        TotalPrice = totalPrice
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

                    foreach (var item in cartItems)
                    {
                        db.OrderItems.Add(new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Product.Price
                        });
                    }

                    db.CartItems.RemoveRange(cartItems);
                    db.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
