using System.ComponentModel.DataAnnotations;

namespace WineryAPI.Models
{
	public class Wine : Base
	{
		public Guid WineryId { get; set; }
		[Required]
		public WineColor Color { get; set; }
		[StringLength(4)]
		public string Vintage { get; set; }
		[Required]
		public decimal Price { get; set; }
		[Required]
		public string IssueDate { get; set; }
		public string Note { get; set; }
	}

	public enum WineColor
	{
		Blush,
		Champagne,
		Dessert,
		Red,
		Rose,
		Sparkling,
		White
	}
}
