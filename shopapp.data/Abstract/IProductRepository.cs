/*
* IProductRepository.cs is created to define the methods that will be used in the ProductRepository class.
*/
using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IProductRepository: IRepository<Product>
    {
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int pageSize, int page);
        List<Product> GetSearchResult(string searchString);
        List<Product> GetHomePageProducts();
        Product GetByIdWithCategories(int id);
        int GetCountByCategory(string category);
        void Update(Product entity, int[] categoryIds);
    }
}