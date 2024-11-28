using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZeraAPI.Model;

namespace ZeraAPI.ZeraAPI.Model
{

    public class WhishList
    {
        [Key]
        public int WhishListId { get; set; }
        public string Id { get; set; }
        public UserCustomised? userCustomised { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<WishListItem> wishListItems{ get; set; } = new List<WishListItem>();
    }


}