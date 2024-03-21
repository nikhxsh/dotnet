using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WineryAPI.Storage.Models
{
	[Table("Wines")]
	public class WineDTO
	{
		[Key]
		[Column("Id")]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public WineColorDTO Color { get; set; }
		public string Vintage { get; set; }
		public decimal Price { get; set; }
		public string IssueDate { get; set; }
		public string Note { get; set; }
		public Guid WineryId { get; set; }
	}

	public enum WineColorDTO
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
