using System;
using System.Collections.Generic;
using System.Linq;

namespace ZeraAPI.ZeraAPI.Model
{

    public class WishListItem
    {
        public int WishListItemId   { get; set; }

        public WhishList? wishList{ get; set; }
        public int WhishListId { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }


}