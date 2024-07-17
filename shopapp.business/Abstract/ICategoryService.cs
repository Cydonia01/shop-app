using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface ICategoryService: IValidator<Category>
    {
        List<Category> GetAll();
        Category GetById(int id);
        Category GetByIdWithProducts(int categoryId);
        bool Create(Category entity);
        bool Update(Category entity);
        void Delete(Category entity);
        void DeleteFromCategory(int productId, int categoryId);
    }
}