﻿using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
		public IActionResult Create(CategoryVM categoryVm)
		{
            if(!ModelState.IsValid)
            {
                return View("Create", categoryVm);
            }

            var category = new Category
            {
                Name = categoryVm.Name
            };
            try
            {
				context.Categories.Add(category);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
            catch
            {
                ModelState.AddModelError("Name", "Category Name Already Exist");
                return View(categoryVm);
            }
            
		}

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            var viewModel = new CategoryVM
            { 
                Id = id,
                Name = category.Name
            };
            return View("Create", viewModel);
        }
        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if(!ModelState.IsValid)
            {
                return View("Create", categoryVM);
            }
            var category = context.Categories.Find(categoryVM.Id);
            if(category is null) 
            {
                return NotFound();
            }

            category.Name = categoryVM.Name;
            category.UpdatedOn = DateTime.Now;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var category = context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            var viewModel = new CategoryVM
            {
                Id = id,
                Name= category.Name,
                CreatedOn = category.CreatedOn,
                UpdatedOn = category.UpdatedOn,
            };
            return View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            var category = context.Categories.Find(id);
            if(category is null)
            {
                return NotFound();
            }

            context.Categories.Remove(category);
            context.SaveChanges();

            return Ok();
        }
    }
}
