using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ZeraAPI.ZeraAPI.Model
{

    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public Order? order { get; set; }
    }


}