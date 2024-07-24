using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.data.Concrete
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order>, IOrderRepository
    {

        public EfCoreOrderRepository(ShopContext context) : base(context)
        {
            
        }
        private ShopContext ShopContext {
            get { return context as ShopContext; }
        }
        public List<Order> GetOrders(string userId)
        {
            var orders = ShopContext.Orders
                .Include(i => i.OrderItems)
                .ThenInclude(i => i.Product)
                .Include(i => i.ShippingAddress)
                .Include(i => i.BillingAddress)
                .Include(i => i.Card)
                .AsQueryable();

            if(!string.IsNullOrEmpty(userId)) {
                orders = orders.Where(i => i.UserId == userId);
            }
            return orders.ToList();
        }
    }
}