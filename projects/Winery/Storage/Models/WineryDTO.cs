using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WineryAPI.Storage.Models
{
	[Table("Wineries")]
	public class WineryDTO
	{
		[Key]
		[Column("Id")]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }
	}
}
