
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using WpfApp10.Data;
using WpfApp10.Models;

namespace WpfApp10.Services
{
    public class CartService
    {
        public List<CartItem> GetCart()
        {
            using (var db = new ShopContext())
            {
                return db.CartItems
                    .Include(c => c.Product)
                    .ToList();
            }
        }

        public void AddToCart(int productId)
        {
            using (var db = new ShopContext())
            {
                var item = db.CartItems
                    .FirstOrDefault(c => c.ProductId == productId);

                if (item == null)
                {
                    db.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        Quantity = 1
                    });
                }
                else
                {
                    item.Quantity++;
                }

                db.SaveChanges();
            }
        }

        public void ChangeQuantity(int cartItemId, int quantity)
        {
            using (var db = new ShopContext())
            {
                var item = db.CartItems.Find(cartItemId);

                if (item == null) return;

                if (quantity <= 0)
                    db.CartItems.Remove(item);
                else
                    item.Quantity = quantity;

                db.SaveChanges();
            }
        }

        public void RemoveFromCart(int cartItemId)
        {
            using (var db = new ShopContext())
            {
                var item = db.CartItems.Find(cartItemId);

                if (item != null)
                {
                    db.CartItems.Remove(item);
                    db.SaveChanges();
                }
            }
        }

        public void ClearCart()
        {
            using (var db = new ShopContext())
            {
                db.CartItems.RemoveRange(db.CartItems.ToList());
                db.SaveChanges();
            }
        }
    }
}
