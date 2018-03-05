using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Entities;

namespace WebShop.Domain.Abstract
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
        IEnumerable<OrderItem> OrderItems { get; }
        void SaveOrder(Order customerDetails);
        Order DispatchOrder(int orderId);
    }
}
