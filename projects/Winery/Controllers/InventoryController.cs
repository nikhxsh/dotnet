using Microsoft.AspNetCore.Mvc;
using WineryAPI.Models;
using WineryAPI.Service;
using WineryStore.API.Filters;

namespace WineryAPI.Controllers
{
	[Route("api/winery/{wineryId}/wine")]
	[ApiController]
	[CustomHttpsOnlyFilter]
	public class InventoryController : Controller
	{
		private readonly IInventoryService inventoryService;

		public InventoryController(IInventoryService inventoryService)
		{
			this.inventoryService = inventoryService;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			[FromRoute] Guid WineryId,
			[FromQuery] int skip = 0,
			[FromQuery] int take = 10
			)
		{
			try
			{
				var request = new FetchRequest
				{
					Skip = skip,
					Take = take
				};
				var wines = await inventoryService.ListWinesAsync(WineryId, request);
				return Ok(wines);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{wineId}")]
		public async Task<IActionResult> Get(Guid wineryId, Guid wineId)
		{
			try
			{
				var wine = await inventoryService.FetchWineAsync(wineryId, wineId);
				return Ok(wine);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
