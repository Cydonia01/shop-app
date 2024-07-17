using shopapp.business.Abstract;
using shopapp.entity;
using System.Collections.Generic;
using shopapp.data.Abstract;
using System.Runtime.InteropServices;

namespace shopapp.business.Concrete
{
    public class CategoryManager: ICategoryService
    {
        private ICategoryRepository _categoryRepository;


        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _categoryRepository.GetByIdWithProducts(categoryId);
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public bool Create(Category entity)
        {
            if (Validate(entity)) {
                _categoryRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);
        }

        public bool Update(Category entity)
        {
            if (Validate(entity)) {
                _categoryRepository.Update(entity);
                return true;
            }
            return false;
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _categoryRepository.DeleteFromCategory(productId, categoryId);
        }

        public string ErrorMessage { get; set; }
        public bool Validate(Category entity)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Category name is required.<br/>";
                isValid = false;
            }
            if (string.IsNullOrEmpty(entity.Url))
            {
                ErrorMessage += "Category url is required.<br/>";
                isValid = false;
            }
            return isValid;
        }
    }
}