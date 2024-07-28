/*
* EfCoreOrderRepository class is created for the Order entity to implement the IOrderRepository interface.
*/

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

        public EfCoreOrderRepository(ShopContext context) : base(context) {}
        
        // Define the ShopContext property
        private ShopContext ShopContext {
            get { return context as ShopContext; }
        }

        // Gets the orders by the user id
        public List<Order> GetOrders(string userId)
        {
            var orders = ShopContext.Orders
                .Include(i => i.OrderItems) // Same as joining the OrderItems table
                .ThenInclude(i => i.Product) // Same as joining the Product table
                .Include(i => i.ShippingAddress) // Same as joining the ShippingAddress table
                .Include(i => i.BillingAddress) // Same as joining the BillingAddress table
                .Include(i => i.Card) // Same as joining the Card table
                .AsQueryable();

            if(!string.IsNullOrEmpty(userId)) {
                orders = orders.Where(i => i.UserId == userId);
            }
            return orders.ToList();
        }
    }
}