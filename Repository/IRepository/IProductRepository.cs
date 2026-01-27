using System;
using ApiEcommerce.Models;

// 1. Crear una interfaz llamada IProductRepository.
namespace ApiEcommerce.Repository.IRepository;

// 2. Incluir los siguientes métodos en la interfaz:
public interface IProductRepository
{
    //    - GetProducts
    //        → Devuelve todos los productos en ICollection del tipo Product.
    ICollection<Product> GetProducts();

    //    - GetProductsForCategory
    //        → Recibe un categoryId y devuelve los productos de esa categoría en ICollection del tipo Product.
    ICollection<Product> GetProductsForCategory(int categoryId);

    //    - SearchProduct
    //        → Recibe un nombre y devuelve los productos que coincidan en ICollection del tipo Product.
    ICollection<Product> SearchProducts(string searchTerm);

    //    - GetProduct
    //        → Recibe un id y devuelve un solo objeto Product o null si no se encuentra.
    Product? GetProduct(int id);

    //    - BuyProduct
    //        → Recibe el nombre del producto y una cantidad, y devuelve un bool indicando si la compra fue exitosa.
    bool BuyProduct(string name, int quantity);

    //    - ProductExists (por id)
    //        → Recibe un id y devuelve un bool indicando si existe el producto.
    bool ProductExists(int id);

    //    - ProductExists (por nombre)
    //        → Recibe un nombre y devuelve un bool indicando si existe el producto.
    bool ProductExists(string name);

    //    - CreateProduct
    //        → Recibe un objeto Product y devuelve un bool indicando si la creación fue exitosa.
    bool CreateProduct(Product product);

    //    - UpdateProduct
    //        → Recibe un objeto Product y devuelve un bool indicando si la actualización fue exitosa.
    bool UpdateProduct(Product product);

    //    - DeleteProduct
    //        → Recibe un objeto Product y devuelve un bool indicando si la eliminación fue exitosa.
    bool DeleteProduct(Product product);
    
    //    - Save
    //        → Devuelve un bool indicando si los cambios se guardaron correctamente.
    bool Save();
}
