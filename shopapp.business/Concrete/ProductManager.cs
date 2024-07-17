using System.Collections.Generic;
using shopapp.entity;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using System.ComponentModel.DataAnnotations;
using System;

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

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }
        public bool Create(Product entity)
        {
            if(Validate(entity)) {
                _productRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public bool Update(Product entity)
        {
            if(Validate(entity)) {
                _productRepository.Update(entity);
                return true;
            }
            return false;
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validate(entity))
            {
                if(categoryIds.Length == 0) {
                    ErrorMessage += "Please select at least one category\n";
                    return false;
                }
                _productRepository.Update(entity, categoryIds);
                return true;
            }
            return false;
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public string ErrorMessage { get; set; }
        public bool Validate(Product entity)
        {
            var isValid = true;

            if(string.IsNullOrEmpty(entity.Name)) {
                ErrorMessage += "Product name is required.<br/>";
                isValid = false;
            }
            // string length
            if (string.IsNullOrEmpty(entity.Price.ToString())) {
                ErrorMessage += "Price is required.<br/>";
                isValid = false;
            }
            if (entity.Price < 0) {
                ErrorMessage += "Price cannot be negative.<br/>";
                isValid = false;
            }
            //price length
            if (string.IsNullOrEmpty(entity.ImageUrl)) {
                ErrorMessage += "Image URL is required.<br/>";
                isValid = false;
            }
            if (string.IsNullOrEmpty(entity.Description)) {
                ErrorMessage += "Description is required.<br/>";
                isValid = false;
            }
            //description length
            if(string.IsNullOrEmpty(entity.Url)) {
                ErrorMessage += "Url is required.<br/>";
                isValid = false;
            }
            //url length
            
            return isValid;
        }
    }
}