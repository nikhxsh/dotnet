using System.ComponentModel.DataAnnotations;

namespace WineryAPI.Models
{
	public abstract class Base
	{
		public Guid Id { get; set; }
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
	}
}
