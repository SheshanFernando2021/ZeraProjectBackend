using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZeraAPI.Model;
using ZeraAPI.ZeraAPI.Model;
using ZeraAPI.ZeraAPI.viewModels;

namespace ZeraAPI.ZeraAPI.Data
{

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public DbSet<Login> logins { get; set; }
        public DbSet<Register> registers { get; set; }

        //the models
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<WhishList> wishLists { get; set; }
        public DbSet<WishListItem> wishListItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

    }


}