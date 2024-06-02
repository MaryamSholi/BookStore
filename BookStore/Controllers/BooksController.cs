using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace BookStore.Controllers
{
	public class BooksController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment webHostEnvironment;

		public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			this.context = context;
			this.webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			var books = context.Books.
				Include(book => book.Author).
				Include(book => book.Categories).
				ThenInclude(book => book.category).ToList();

			var bookVM = books.Select(book => new BookVM
			{
				Id = book.Id,
				Title = book.Title,
				Author = book.Author.Name,
				Publisher = book.Publisher,
				PublishDate = book.PublishDate,
				ImageUrl = book.ImageUrl,
				Categories = book.Categories.Select(book => book.category.Name).ToList(),
			}).ToList();


			return View(bookVM);
		}
		[HttpGet]
		public IActionResult Create()
		{
			var authors = context.Authors.OrderBy(author =>author.Name).ToList();
			var categories = context.Categories.OrderBy(author => author.Name).ToList();

			var authorList = authors.Select(author => new SelectListItem
			{
				Value = author.Id.ToString(),
				Text = author.Name
			}).ToList();

			var categoryList = categories.Select(category => new SelectListItem
			{
				Value = category.Id.ToString(),
				Text = category.Name
			}).ToList();

			var viewModel = new BookFormVM
			{
				Author = authorList,
				Categories= categoryList,
			};

			return View(viewModel);
		}
		[HttpPost]
		public IActionResult Create(BookFormVM bookFormVM)
		{
			if(!ModelState.IsValid)
			{
				return View(bookFormVM);
			}
			string imageName = null;
			if(bookFormVM.ImageUrl !=  null)
			{
			    imageName = Path.GetFileName(bookFormVM.ImageUrl.FileName);
				var imagePath = Path.Combine($"{webHostEnvironment.WebRootPath}/img/Books",imageName);
				var stream = System.IO.File.Create(imagePath);
				bookFormVM.ImageUrl.CopyTo(stream);
			}
			var book = new Book
			{
				Title = bookFormVM.Title,
				AuthorId = bookFormVM.AuthorId,
				Publisher = bookFormVM.Publisher,
				PublishDate = bookFormVM.PublishDate,
				ImageUrl = imageName,
				Descreption = bookFormVM.Descreption,
				Categories = bookFormVM.SelectedCategories.Select(id => new BookCategory
				{
					CategoryId = id,
				}).ToList(),
			};

			context.Add(book);
			context.SaveChanges();

			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			var book = context.Books.Find(id);
			if(book == null)
			{
				return NotFound();
			}
			var imagePath = Path.Combine($"{webHostEnvironment.WebRootPath}/img/Books", book.ImageUrl);
			if(System.IO.File.Exists(imagePath)) 
			{
				System.IO.File.Delete(imagePath);
			}

			context.Remove(book);
			context.SaveChanges();

			return RedirectToAction("Index");


		}
	}
}
