using Microsoft.AspNetCore.Mvc;
using WineryAPI.Models;
using WineryAPI.Service;

namespace RestService.Controllers
{
	[Route("api/wine")]
	[ApiController]
	public class WineController : Controller
	{
		private readonly IWineService wineService;

		public WineController(IWineService wineService)
		{
			this.wineService = wineService;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			[FromQuery] int skip, 
			[FromQuery] int take = 10
			)
		{
			try
			{
				var fetchRequest = new FetchRequest
				{
					Skip = skip,
					Take = take
				};

				var allWines = await wineService.GetAllWinesAsync(fetchRequest);

				return Ok(allWines);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{wineId:Guid}")]
		public async Task<IActionResult> Get(Guid wineId)
		{
			try
			{
				var existingWine = await wineService.WineExistsAsync(wineId);

				if (!existingWine)
					return NotFound($"Wine with Id {wineId} does not exists.");

				var wine = await wineService.GetWineByIdAsync(wineId);
				return Ok(wine);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post(Wine wine)
		{
			try
			{
				var response = await wineService.AddWineAsync(wine);

				if (response)
				{
					return Created($"api/wines/{wine.Id}", wine);
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Add Wine!");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Put(Wine wine)
		{
			try
			{
				var existingWine = await wineService.WineExistsAsync(wine.Id);

				if (!existingWine)
					return NotFound($"Wine with Id {wine.Id} does not exists.");

				var response = await wineService.UpdateWineAsync(wine);

				if (response)
					return Ok(wine);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Wine!");

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{wineId:Guid}")]
		public async Task<IActionResult> Delete(Guid wineId)
		{
			try
			{
				var existingWine = await wineService.WineExistsAsync(wineId);

				if (!existingWine)
					return NotFound($"Wine with Id {wineId} does not exists.");

				var response = await wineService.RemoveWineAsync(wineId);

				if (response)
					return Ok(wineId);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to remove Wine!");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}