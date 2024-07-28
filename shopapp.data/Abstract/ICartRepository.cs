/*
* ICartRepository.cs is used to define the interface for CartRepository class.
*/
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ICartRepository: IRepository<Cart>
    {
        void ClearCart(int cartId);
        void DeleteFromCart(int cartId, int productId);
        Cart GetByUserId(string userId);
    }
}