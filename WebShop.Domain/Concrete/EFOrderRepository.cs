using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Abstract;
using WebShop.Domain.Entities;
using System.Data.Entity;

namespace WebShop.Domain.Concrete
{
    public class EFOrderRepository : IOrderRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Order> Orders
        {
            get { return context.Orders.Include(o => o.OrderItems.Select(ol => ol.Product)); }
        }

        public IEnumerable<OrderItem> OrderItems
        {
            get { return context.OrderItems; }
        }

        public Order DispatchOrder(int orderId)
        {
            Order dbEntry = context.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (orderId != 0)
            {
                if (dbEntry != null)
                {
                    dbEntry.Dispatched = true;
                }
            }
            context.SaveChanges();
            return dbEntry;
        }

        public void SaveOrder(Order order)
        {
            if (order.OrderId == 0)
            {
                context.Orders.Add(order);

                foreach (OrderItem item in order.OrderItems)
                {
                    context.Entry(item.Product).State
                        = EntityState.Modified;
                }
            }
            else
            {
                Order dbEntry = context.Orders.Find(order.OrderId);
                if (dbEntry != null)
                {
                    dbEntry.Name = order.Name;
                    dbEntry.Address = order.Address;
                    dbEntry.GiftWrap = order.GiftWrap;
                    dbEntry.Dispatched = (bool)false;
                }
            }
            context.SaveChanges();
        }
    }
}
