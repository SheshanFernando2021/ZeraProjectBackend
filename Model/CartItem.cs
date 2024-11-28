using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ZeraAPI.ZeraAPI.Model
{

    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } = 0;

        //Navigation Properties Cart
        public string CartId { get; set; } = string.Empty;
        public Cart? cart { get; set; }

        //Navigation Properties Product
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Product? product { get; set; }
    }


}