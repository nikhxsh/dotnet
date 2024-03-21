using System.ComponentModel.DataAnnotations;

namespace WineryAPI.Models
{
	public class Winery : Base
	{
		[Required]
		[StringLength(50)]
		public string Region { get; set; }

		[Required]
		[StringLength(20)]
		public string Country { get; set; }
	}
}
