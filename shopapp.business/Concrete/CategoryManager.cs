using shopapp.business.Abstract;
using shopapp.entity;
using System.Collections.Generic;
using shopapp.data.Abstract;

namespace shopapp.business.Concrete
{
    public class CategoryManager: ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public void Create(Category entity)
        {
            _categoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);
        }

        public List<Category> GetPopularCategories()
        { 
            return _categoryRepository.GetPopularCategories();
        }
    }
}