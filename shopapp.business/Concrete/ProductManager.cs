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
        private readonly IUnitOfWork _unitOfWork;
    
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public List<Product> GetAll()
        {
            return _unitOfWork.Products.GetAll();
        }

        public Product GetById(int id)
        {
            return _unitOfWork.Products.GetById(id);
        }

        public Product GetProductDetails(string url)
        {
            return _unitOfWork.Products.GetProductDetails(url);
        }
        
        public List<Product> GetProductsByCategory(string name, int pageSize, int page)
        {
            return _unitOfWork.Products.GetProductsByCategory(name, pageSize, page);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _unitOfWork.Products.GetSearchResult(searchString);
        }
        public bool Create(Product entity)
        {
            if(Validate(entity)) {
                _unitOfWork.Products.Create(entity);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public void Delete(Product entity)
        {
            _unitOfWork.Products.Delete(entity);
            _unitOfWork.Save();
        }

        public bool Update(Product entity)
        {
            if(Validate(entity)) {
                _unitOfWork.Products.Update(entity);
                _unitOfWork.Save();
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
                _unitOfWork.Products.Update(entity, categoryIds);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public int GetCountByCategory(string category)
        {
            return _unitOfWork.Products.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _unitOfWork.Products.GetHomePageProducts();
        }

        public Product GetByIdWithCategories(int id)
        {
            return _unitOfWork.Products.GetByIdWithCategories(id);
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