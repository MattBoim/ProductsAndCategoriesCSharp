using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductsAndCatagories.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ProductsAndCatagories.Controllers
{
    public class HomeController : Controller
    {
        private ProductContext dbContext;

        public HomeController(ProductContext context)
        {
            dbContext = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("/products")]
        public IActionResult ProdPage()
        {
            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.Products = AllProducts;
            return View("Index");
        }

        [HttpGet("/categories")]
        public IActionResult CatPage()
        {
            List<Category> AllCategories = dbContext.Categories.ToList();
            ViewBag.Categories = AllCategories;
            return View("Categories");
        }

        [HttpPost("/addproduct")]
        public IActionResult AddProduct(Product product)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(product);
                dbContext.SaveChanges();
                return RedirectToAction("ProdPage");
            }
            return RedirectToAction("ProdPage");
        }

        [HttpPost("/addcategory")]
        public IActionResult AddCat(Category category)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categories.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction("CatPage");
            }
            return RedirectToAction("CatPage");
        }

        [HttpGet("/catagories/{id}")]
        public IActionResult Details(int id)
        {
            var Category = dbContext.Categories.Where(cat => cat.CategoryId == id).FirstOrDefault();
            ViewBag.category = Category.Name;
            var Products = dbContext.Products.ToList();
            ViewBag.Products = Products;
            var RelatedProducts = dbContext.Categories.Where(cat => cat.CategoryId == id).Include(cat => cat.Associations).ThenInclude(ass => ass.Product).FirstOrDefault();
            
            ViewBag.RelatedProducts = RelatedProducts;
            ViewBag.id = id;
            
            return View("CatagoriesDetail");
        }

        [HttpGet("/products/{id}")]
        public IActionResult ProdDetails(int id)
        {   
            var categories = dbContext.Categories.ToList();
            ViewBag.Categories = categories;

            var selprod = dbContext.Products.Where(prod => prod.ProductId == id).FirstOrDefault();
            ViewBag.productName = selprod.Name;

            var RelatedCategories = dbContext.Products.Where(prod => prod.ProductId == id).Include(prod => prod.Associations).ThenInclude(ass => ass.Category).FirstOrDefault();

            ViewBag.RelatedCategories = RelatedCategories;
            ViewBag.id = id;
            
            return View("ProductsDetail");
        }

        [HttpPost("/attachprod{id}")]
        public IActionResult AttachProd(int ProductId, int id)
        {
            System.Console.WriteLine("PRODUCT ID: "+ProductId+", Category ID: "+id);
            if (ModelState.IsValid)
            {
                Associations association = new Associations
                {
                    ProductId = ProductId,
                    CategoryId = id
                };
                
                dbContext.Associations.Add(association);

                dbContext.SaveChanges();
            }
            return RedirectToAction("Details");
        }

        [HttpPost("/attachcat{id}")]
        public IActionResult AttachCat(int CategoryId, int id)
        {
            System.Console.WriteLine("PRODUCT ID: "+id+", Category ID: "+CategoryId);
            if (ModelState.IsValid)
            {
                Associations association = new Associations
                {
                    ProductId = id,
                    CategoryId = CategoryId
                };
                
                dbContext.Associations.Add(association);

                dbContext.SaveChanges();
            }
            return RedirectToAction("ProdDetails");
        }
    }
}
