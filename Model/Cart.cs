using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZeraAPI.Model;

namespace ZeraAPI.ZeraAPI.Model
{

    public class Cart
    {
        [Key]
        public string CartId { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
    }
}