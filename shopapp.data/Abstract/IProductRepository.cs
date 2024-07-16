using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IProductRepository: IRepository<Product>
    {
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int pageSize, int page);
        List<Product> GetPopularProducts();
        int GetCountByCategory(string category);
    }
}