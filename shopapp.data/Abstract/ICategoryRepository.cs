/*
* ICategoryRepository.cs is used to implement the methods of the CategoryRepository class.
*/
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByIdWithProducts(int categoryId);
        void DeleteFromCategory(int productId, int categoryId);
    }
}