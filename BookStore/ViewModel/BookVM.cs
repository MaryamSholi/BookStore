using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
	public class BookVM
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public String Author { get; set; } = null!;
		public string Publisher { get; set; } = null!;
		public DateTime PublishDate { get; set; } = DateTime.Now;
		public string ImageUrl { get; set; }
		public List<string> Categories { get; set; }
	}
}
