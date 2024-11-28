using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZeraAPI.Model;

namespace ZeraAPI.ZeraAPI.Model
{

    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [DisplayName("Product Name")]
        public required string ProductName { get; set; }
        [DisplayName("Product type")]
        public required string Producttype { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required string ImageURL { get; set; }

        public List<CartItem>? cartItem { get; set; } = new List<CartItem>();
        public List<OrderItem>? orderItem { get; set; } = new List<OrderItem>();
        public List<WishListItem>? wishListItem { get; set; } = new List<WishListItem>();
    }

}