
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp10.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [StringLength(500)]
        public string ImagePath { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
