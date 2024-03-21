using WineryAPI.Models;

namespace WineryAPI.Service
{
	public interface IInventoryService
	{
		Task<PagedResponse<IEnumerable<Wine>>> ListWinesAsync(Guid wineryId, FetchRequest request);
		Task<Wine> FetchWineAsync(Guid wineryId, Guid wineId);
	}
}
