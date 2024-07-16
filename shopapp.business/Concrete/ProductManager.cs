using System.Collections.Generic;
using shopapp.entity;
using shopapp.business.Abstract;
using shopapp.data.Abstract;

namespace shopapp.business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }
        
        public List<Product> GetProductsByCategory(string name, int pageSize, int page)
        {
            return _productRepository.GetProductsByCategory(name, pageSize, page);
        }

        public void Create(Product entity)
        {
            _productRepository.Create(entity);
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }
    }
}