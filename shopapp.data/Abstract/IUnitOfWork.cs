/*
* This file contains the IUnitOfWork interface.
* This interface is used to define the methods that are required to be implemented by the UnitOfWork class.
*/
using System;

namespace shopapp.data.Abstract
{
    public interface IUnitOfWork: IDisposable
    {
        ICartRepository Carts { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        void Save();
    }
}