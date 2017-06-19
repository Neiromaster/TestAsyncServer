using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBaseModel
{
    public class People
    {
        public int Id { get; set; }

        [MaxLength(32)]
        [Required]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(32)]
        [Required]
        public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public People()
        {
            Orders = new List<Order>();
        }
    }
}