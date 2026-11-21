
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp10.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [NotMapped]
        public decimal Sum
        {
            get
            {
                return Product == null ? 0 : Product.Price * Quantity;
            }
        }
    }
}
