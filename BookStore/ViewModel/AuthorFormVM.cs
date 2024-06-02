using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
	public class AuthorFormVM
	{
		public int Id { get; set; }
		[MaxLength(50,ErrorMessage ="The Name Should Be 50 Characters")]
		public string Name { get; set; } = null!;

	}
}
