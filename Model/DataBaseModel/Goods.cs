using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBaseModel
{
    public class Goods
    {
        public int Id { get; set; }

        [MaxLength(32)]
        [Index(IsUnique = true)]
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public Goods()
        {
            Orders = new List<Order>();
        }
    }
}