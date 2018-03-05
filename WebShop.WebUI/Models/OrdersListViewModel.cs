using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Domain.Entities;

namespace WebShop.WebUI.Models
{
    public class OrdersListViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}