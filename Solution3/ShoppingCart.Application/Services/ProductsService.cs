using System;
using ShoppingCart.Application.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Data.Repositories;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ShoppingCart.Application.Services
{
    public class ProductsService : IProductsService
    {
        private IProductsRepository _productsRepo;
        private IMapper _mapper;
        public ProductsService(IProductsRepository productsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productsRepo = productsRepository; //dependency injections
        }

        public void AddProduct(ProductViewModel product)
        { 
            var myProduct = _mapper.Map<Product>(product);
            myProduct.Category = null;
            _productsRepo.AddProduct(myProduct);
        }

        public void DeleteProduct(Guid id)
        {
            var pToDelete = _productsRepo.GetProduct(id);
            if (pToDelete != null)
            {
                _productsRepo.DeleteProduct(pToDelete);
            }
        }

        public void Enable(Guid id)
        {
            var pToEnable = id;
            if (pToEnable != null)
            {
                _productsRepo.Enable(pToEnable); 
            }

        }

        public void Disable(Guid id)
        {
            var pToDisable = id;
            if (pToDisable != null)
            {
                _productsRepo.Disable(pToDisable); 
            }
            
        }

        public ProductViewModel GetProduct(Guid id)
        {
            var myProduct = _productsRepo.GetProduct(id);
            var result = _mapper.Map<ProductViewModel>(myProduct);
            return result;
        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            //check whether automapper works

            var products = _productsRepo.GetProducts().ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);
            
            return products;

        }

        public IQueryable<ProductViewModel> GetProducts(string keyword)
        {
            //check whether automapper works

            var products = _productsRepo.GetProducts().Where(x=>x.Description.Contains(keyword) || x.Name.Contains(keyword))
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);

            return products;

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
