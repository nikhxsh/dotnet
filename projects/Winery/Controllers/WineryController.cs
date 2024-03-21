using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WineryAPI.Models;
using WineryAPI.Service;

namespace WineryAPI.Controllers
{
	[Route("api/winery")]
	[ApiController]
	public class WineryController : Controller
	{
		private readonly IWineryService wineryService;

		public WineryController(IWineryService wineryService)
		{
			this.wineryService = wineryService;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			[FromQuery] int skip = 0,
			[FromQuery] int take = 10
			)
		{
			var request = new FetchRequest
			{
				Skip = skip,
				Take = take
			};

			try
			{
				var wineries = await wineryService.GetAllWineriesAsync(request);
				return Ok(wineries);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		[HttpGet("{wineryId:Guid}")]
		public async Task<IActionResult> Get(Guid wineryId)
		{
			try
			{
				var winery = await wineryService.GetWinerybyIdAsync(wineryId);

				if (winery == null)
					return NotFound($"Winery with Id {wineryId} does not exists.");

				return Ok(winery);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post(Winery winery)
		{
			try
			{
				winery.Id = Guid.NewGuid();

				var response = await wineryService.AddWineryAsync(winery);

				if (response)
				{
					return Created($"api/winery/{winery.Id}", winery);
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Add Winery!");
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Put(Winery winery)
		{
			try
			{
				var wineryExists = await wineryService.WineryExistsAsync(winery.Id);

				if (!wineryExists)
					return NotFound($"Winery with Id {winery.Id} does not exists.");

				var response = await wineryService.UpdateWineryAsync(winery);

				if (response)
					return Ok(winery);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Winery!");
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPatch("{wineryId}")]
		public async Task<IActionResult> Patch(Guid wineryId, [FromBody] JsonPatchDocument<Winery> wineryPatch)
		{
			try
			{
				var wineryExists = await wineryService.WineryExistsAsync(wineryId);

				if (!wineryExists)
					return NotFound($"Winery with Id {wineryId} does not exists.");

				Winery winery = new();
				wineryPatch.ApplyTo(winery);

				return Ok(winery);
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{wineryId}")]
		public async Task<IActionResult> Delete(Guid wineryId)
		{
			try
			{
				var wineryExists = await wineryService.WineryExistsAsync(wineryId);

				if (!wineryExists)
					return NotFound($"Winery with Id {wineryId} does not exists.");

				var response = await wineryService.RemoveWineryAsync(wineryId);

				if (response)
					return Ok(wineryId);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to remove Winery!");
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
