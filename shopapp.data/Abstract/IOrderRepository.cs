/*
* IOrderRepository.cs is used to define the interface for OrderRepository
*/
using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}