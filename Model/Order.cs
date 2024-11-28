using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZeraAPI.Model;

namespace ZeraAPI.ZeraAPI.Model
{

    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [DisplayName("Order date")]
        public required DateTime OrderDate { get; set; }
        [DisplayName("Shipping Date")]
        public required DateTime ShippingDate { get; set; }
        [DisplayName("Shipping Address")]
        public required string ShippingAddress { get; set; }
        [DisplayName("Total Amount")]
        public required decimal TotalAmount { get; set; }
        [DisplayName("Order Status")]
        public OrderStatus status { get; set; }

        //Navigation Properties
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
#pragma warning disable CS8618 
        public string Id { get; set; }
#pragma warning restore CS8618 
        public UserCustomised? userCustomised { get; set; }
        public enum OrderStatus
        {
            Pending,
            Shipped,
            Delivered,
            Canceled
        }
    }


}