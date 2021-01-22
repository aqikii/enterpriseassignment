﻿using System;
using ShoppingCart.Application.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Data.Repositories;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.Services
{
    public class ProductsService : IProductsService
    {
        private IProductsRepository _productsRepo;
        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepo = productsRepository;
        }

        public void AddProduct(ProductViewModel product)
        { //automapper
            Product newProduct = new Product()
            {

                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.Category.Id,
                ImageUrl = product.ImageUrl
            };


            newProduct.Name = product.Name;
            _productsRepo.AddProduct(newProduct);
        }

        public void DeleteProduct(Guid id)
        {
            var pToDelete = _productsRepo.GetProduct(id);
            if (pToDelete != null)
            {
                _productsRepo.DeleteProduct(pToDelete);
            }
        }

        /* public void DisableProduct(Guid id)
         {
            var pToDisable = _productsRepo.GetProduct(id);
             if (pToDisable is not disabled)
             {
                 _productsRepo.Disableproduct(pToDisable); not official pls chekc code (written from lesson 10)
             }
             throw new NotImplementedException();
         }*/

        public ProductViewModel GetProduct(Guid id)
        {
            //automapper
            var myProduct = _productsRepo.GetProduct(id);
            ProductViewModel myModel = new ProductViewModel();
            myModel.Description = myProduct.Description;
            myModel.ImageUrl = myProduct.ImageUrl;
            myModel.Name = myProduct.Name;
            myModel.Price = myProduct.Price;
            myModel.Id = myProduct.Id;
            myModel.Category = new CategoryViewModel()
            {
                Id = myProduct.Category.Id,
                Name = myProduct.Category.Name
            };
            return myModel;
        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            //tobeimplemented using automapper



            var list = from p in _productsRepo.GetProducts()
                       select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name },
                           ImageUrl = p.ImageUrl

                       };
            return list;
        }

        public IQueryable<ProductViewModel> GetProducts(int category)
        {
            var list = from p in _productsRepo.GetProducts().Where(x => x.Category.Id == category)
            select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name },
                           ImageUrl = p.ImageUrl
            };
            return list;
        }
    }
}
