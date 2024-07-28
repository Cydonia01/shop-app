/*
* This class implements the ICategoryService interface.
*/

using shopapp.business.Abstract;
using shopapp.entity;
using System.Collections.Generic;
using shopapp.data.Abstract;

namespace shopapp.business.Concrete
{
    public class CategoryManager: ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Category> GetAll()
        {
            return _unitOfWork.Categories.GetAll();
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _unitOfWork.Categories.GetByIdWithProducts(categoryId);
        }

        public Category GetById(int id)
        {
            return _unitOfWork.Categories.GetById(id);
        }

        public bool Create(Category entity)
        {
            if (Validate(entity)) {
                _unitOfWork.Categories.Create(entity);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public void Delete(Category entity)
        {
            _unitOfWork.Categories.Delete(entity);
            _unitOfWork.Save();
        }

        public bool Update(Category entity)
        {
            if (Validate(entity)) {
                _unitOfWork.Categories.Update(entity);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _unitOfWork.Categories.DeleteFromCategory(productId, categoryId);
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