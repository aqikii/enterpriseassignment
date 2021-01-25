using System;
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

        public IActionResult Search(string keyword)
        {
            var list = _productsService.GetProducts(keyword);
            return View("Index", list);
        }

        public IActionResult Laptops()
        {
            try
            {
                //var list = _productsService.GetProducts().Where(x => x.Category.Id == 1); Please check returns null
                var list = _productsService.GetProducts();
                TempData["warning"] = "Could not sort by Laptops";
                return View("Index", list);
            }catch(Exception ex)
            {
                TempData["warning"] = "Could not sort by Laptops";
                return View("Index");

            }
        }

        public IActionResult Appliances()
        {
            try {
                //var list = _productsService.GetProducts().Where(x => x.Category.Id == 2); Please check returns null
                TempData["warning"] = "Could not sort by appliances";
                var list = _productsService.GetProducts();
                return View("Index", list);
            } catch(Exception ex)
            {
                TempData["warning"] = "Could not sort by appliances";
                return View("Index");
            }
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
                TempData["warning"] = ex;
            }
            var listOfCategories = _categoriesService.GetCategories();
            ViewBag.Categories = listOfCategories;
            return View(data);
        }

        public IActionResult Delete(Guid id)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {

                    _productsService.DeleteProduct(id);
                    TempData["feedback"] = "Product was deleted";
                }
                catch (Exception ex)
                {
                    TempData["warning"] = "Product was not deleted"; //change from viewdata to tempdata
                }
            }else TempData["warning"] = "Product was not deleted";
            return RedirectToAction("Index");
        }

        public IActionResult Disable(Guid id)
        {
            if (User.IsInRole("Admin"))   
         { 
                try
            {

                _productsService.Disable(id);
                TempData["feedback"] = "Product was Disabled";
            }
            catch (Exception ex)
            {
                TempData["warning"] = "Product was not disabled"; //change from viewdata to tempdata
            }
        }else TempData["warning"] = "Product was not disabled";
            return RedirectToAction("Index");
        }

        public IActionResult Enable(Guid id)
        {
            if (User.IsInRole("Admin"))
            {
                try
                {

                    _productsService.Enable(id);
                    TempData["feedback"] = "Product was Enabled";
                }
                catch (Exception ex)
                {
                    TempData["warning"] = "Product was not enabled"; 
                }
            }else TempData["warning"] = "Product was not enabled";
            return RedirectToAction("Index");
        }
    }


}