/*
* This file includes the interface of the product service.
* The interface includes the methods that are used in the product service.
*/
using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IProductService : IValidator<Product>
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product GetByIdWithCategories(int id);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int pageSize, int page);
        List<Product> GetHomePageProducts();

        List<Product> GetSearchResult(string searchString);
        int GetCountByCategory(string category);

        bool Create(Product entity);
        bool Update(Product entity);
        bool Update(Product entity, int[] categoryIds);
        void Delete(Product entity);
    }
}