using ProductInventory.Models;

namespace ProductInventory.Services
{
    public interface IProductService
    {
        ProductListViewModel GetProducts(int page);
        Product GetProductById(int id);
        ServiceResult CreateProduct(Product product);
        ServiceResult UpdateProduct(Product product);
        ServiceResult DeleteProduct(int id);
        bool ProductExists(string name, int? excludeId = null);
    }
}
