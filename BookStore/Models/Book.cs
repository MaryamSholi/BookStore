using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
	public class Book
	{
		public int Id { get; set; }
		[MaxLength(50)]
		public string Title { get; set; } = null!;
		public int AuthorId { get; set; }
		public Author Author { get; set; }
		public string Publisher { get; set; } = null!;
		public DateTime PublishDate { get; set; }
		public string? ImageUrl { get; set; }
		public string Descreption { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
		public List<BookCategory> Categories { get; set; } = new List<BookCategory>();

	}
}
