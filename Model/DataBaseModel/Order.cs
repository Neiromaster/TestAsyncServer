using System;
using System.ComponentModel.DataAnnotations;

namespace Model.DataBaseModel
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }

        [Required]
        public int GoodsId { get; set; }

        public virtual Goods Goods { get; set; }

        [Required]
        public int PeopleId { get; set; }

        public virtual People People { get; set; }

        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public int State { get; set; }
    }
}