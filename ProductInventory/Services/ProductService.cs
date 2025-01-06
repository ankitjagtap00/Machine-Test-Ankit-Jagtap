using Microsoft.EntityFrameworkCore;
using ProductInventory.Data;
using ProductInventory.Models;

namespace ProductInventory.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductListViewModel GetProducts(int page)
        {
            var totalRecords = _context.Products.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

            var products = _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                })
                .ToList();

            return new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = PageSize
            };
        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);
        }

        public ServiceResult CreateProduct(Product product)
        {
            try
            {
                if (ProductExists(product.ProductName))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "A product with this name already exists."
                    };
                }

                if (!_context.Categories.Any(c => c.CategoryId == product.CategoryId))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Selected category does not exist."
                    };
                }

                _context.Products.Add(product);
                _context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Error creating product: {ex.Message}"
                };
            }
        }

        public ServiceResult UpdateProduct(Product product)
        {
            try
            {
                if (ProductExists(product.ProductName, product.ProductId))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "A product with this name already exists."
                    };
                }

                if (!_context.Categories.Any(c => c.CategoryId == product.CategoryId))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Selected category does not exist."
                    };
                }

                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Error updating product: {ex.Message}"
                };
            }
        }

        public ServiceResult DeleteProduct(int id)
        {
            try
            {
                var product = _context.Products.Find(id);

                if (product == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Product not found."
                    };
                }

                _context.Products.Remove(product);
                _context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Error deleting product: {ex.Message}"
                };
            }
        }

        public bool ProductExists(string name, int? excludeId = null)
        {
            return _context.Products.Any(p =>
                p.ProductName.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || p.ProductId != excludeId.Value));
        }
    }
}
