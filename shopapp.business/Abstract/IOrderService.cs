/*
* This interface is used to define the methods that are used in the OrderService class.
*/

using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
    }
}