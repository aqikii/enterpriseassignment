﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private IWebHostEnvironment _env;
        public ProductsController(IProductsService productsService, ICategoriesService categoriesService, IWebHostEnvironment env)
        {
            _productsService = productsService;
            _categoriesService = categoriesService;
            _env = env;
        }

        public IActionResult Index()
        {
            var list = _productsService.GetProducts();

            return View(list);
        }

        public IActionResult Details(Guid id)
        {
            var p = _productsService.GetProduct(id);
            return View(p);
        }

        [HttpGet]
        [Authorize (Roles = "Admin")]
        public IActionResult Create()
        {
            var listOfCategories = _categoriesService.GetCategories();
            ViewBag.Categories = listOfCategories;
            return View();
        }
        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductViewModel data, IFormFile f)
        {
            try
            {
                if (f != null)
                {
                    if (f.Length > 0)
                    {
                        string newFilename = Guid.NewGuid() + System.IO.Path.GetExtension(f.FileName);
                        string newFilenameWithAbsolutePath =_env.WebRootPath + @"\Images\" + newFilename;
                        using (var stream = System.IO.File.Create(newFilenameWithAbsolutePath))
                        {
                            f.CopyTo(stream);
                        }
                        data.ImageUrl = @"\Images\" + newFilename;
                    }
                }

                _productsService.AddProduct(data);
                TempData["feedback"] = "Product added successfully!";
            }catch(Exception ex)
            {
                //errorr
                TempData["warning"] = "Product was not added";
            }
            var listOfCategories = _categoriesService.GetCategories();
            ViewBag.Categories = listOfCategories;
            return View(data);
        }

        public IActionResult Delete(Guid id)
        {
            try
            {

                _productsService.DeleteProduct(id);
                TempData["feedback"] = "Product was deleted";
            }catch(Exception ex)
            {
                TempData["warning"] = "Product was not deleted"; //change from viewdata to tempdata
            }
            return RedirectToAction("Index");
        }
    }


}