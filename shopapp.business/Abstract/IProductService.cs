using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        List<Product> GetAll();
        Product GetProductDetails(int id);
        List<Product> GetProductsByCategory(string name);

        void Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
    }
}