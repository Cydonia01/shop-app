/*
* This class implements the ICartRepository interface.
* It is used to manage the cart operations.
*/
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.data.Concrete
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart>, ICartRepository
    {
        public EfCoreCartRepository(ShopContext context) : base(context) {}

        // Define the ShopContext property
        private ShopContext ShopContext {
            get { return context as ShopContext; }
        }

        // Clears the cart after the user has checked out
        public void ClearCart(int cartId)
        {
            var cmd = @"delete from CartItems where CartId = @p0";
            ShopContext.Database.ExecuteSqlRaw(cmd, cartId);
        }

        // Deletes a product from the cart
        public void DeleteFromCart(int cartId, int productId)
        {
            var cmd = @"delete from CartItems where CartId = @p0 and ProductId = @p1";
            ShopContext.Database.ExecuteSqlRaw(cmd, cartId, productId);
        }

        // Gets the cart by the user id
        public Cart GetByUserId(string userId)
        {
            return ShopContext.Carts
                .Include(i => i.CartItems) // Same as joining the CartItems table
                .ThenInclude(i => i.Product) // Same as joining the Product table
                .FirstOrDefault(i => i.UserId == userId);
        }

        // Updates the cart
        public override void Update(Cart entity)
        {
            ShopContext.Carts.Update(entity);
        }
    }
}