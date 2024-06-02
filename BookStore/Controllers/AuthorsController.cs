using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
	public class AuthorsController : Controller
	{
		private readonly ApplicationDbContext context;

		public AuthorsController(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IActionResult Index()
		{
			var authors= context.Authors.ToList();
			var authorsVm = authors.Select(author => new AuthorVM
			{
				Id = author.Id,
				Name = author.Name,
				CreatedOn = author.CreatedOn,
				UpdatedOn = author.UpdatedOn,
			}).ToList();
			
			return View(authorsVm);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View("Form");
		}
		[HttpPost]
		public IActionResult Create(AuthorFormVM authorFormVM)
		{
			if(!ModelState.IsValid)
			{
				return View("Form", authorFormVM);
			}
			var author = new Author()
			{
				Name = authorFormVM.Name,
			};
			try
			{
				context.Authors.Add(author);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				ModelState.AddModelError("Name", "Auther Name Already Exist");
				return View("Form");
			}




		}
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var author = context.Authors.Find(id);
			if (author == null)
			{
				return NotFound();
			}
			var viewModel = new AuthorVM()
			{
				Id=author.Id,
				Name = author.Name,
			};

			return View("Form",viewModel);
		}
		[HttpPost]
		public IActionResult Edit(AuthorVM authorVM)
		{
			if (!ModelState.IsValid)
			{
				return View("Form", authorVM);
			}
			var author = context.Authors.Find(authorVM.Id);
			if(author == null)
			{
				return NotFound();
			}

			author.Name = authorVM.Name;
			author.UpdatedOn = DateTime.Now;
			context.SaveChanges();

			return RedirectToAction("Index");

		}
		public IActionResult Details(int id)
		{
			var author = context.Authors.Find(id);
			if (author == null)
			{
				return NotFound();
			}
			var viewModel = new AuthorVM()
			{
				Id = author.Id,
				Name = author.Name,	
				CreatedOn = author.CreatedOn,
				UpdatedOn = author.UpdatedOn,
			};
			return View(viewModel);
		}
		public IActionResult Delete(int id)
		{
			var author = context.Authors.Find(id);
			if (author == null)
			{
				return NotFound();
			}
			context.Remove(author);
			context.SaveChanges();	

			return RedirectToAction("Index");
		}
	}
}
