using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IProductsRepository
    {
        IQueryable<Product> GetProducts();

        Product GetProduct(Guid id);

        void DeleteProduct(Product p);

        void Disable(Guid id);

        void Enable(Guid id);

        Guid AddProduct(Product p);
    }
}
