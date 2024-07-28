/*
* The UnitOfWork class is used to define the repositories and save the changes to the database.
*/
using shopapp.data.Abstract;

namespace shopapp.data.Concrete.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        // Define the ShopContext property
        private readonly ShopContext _context;

        // Constructor: Initializes the UnitOfWork class with the ShopContext parameter.
        public UnitOfWork(ShopContext context) { _context = context; }

        // Declare the repositories
        private EfCoreCartRepository _cartRepository;
        private EfCoreCategoryRepository _categoryRepository;
        private EfCoreOrderRepository _orderRepository;
        private EfCoreProductRepository _productRepository;

        // Defines the repositories. If the repositories are null, it creates a new instance of the repository.
        public ICartRepository Carts =>
            _cartRepository = _cartRepository ?? new EfCoreCartRepository(_context);

        public ICategoryRepository Categories => 
            _categoryRepository = _categoryRepository ?? new EfCoreCategoryRepository(_context);

        public IOrderRepository Orders => 
            _orderRepository = _orderRepository ?? new EfCoreOrderRepository(_context);

        public IProductRepository Products => 
            _productRepository = _productRepository ?? new EfCoreProductRepository(_context);

        // Purpose of the Dispose method is to release the resources.
        public void Dispose()
        {
            _context.Dispose();
        }

        // Save method is used to save the changes to the database.
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}