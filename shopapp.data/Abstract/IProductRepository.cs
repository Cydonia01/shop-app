using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IProductRepository: IRepository<Product>
    {
        Product GetProductDetails(int id);
        List<Product> GetProductsByCategory(string name);
        List<Product> GetPopularProducts();
    }
}