using WineryAPI.Models;

namespace WineryAPI.Service
{
	public interface IWineryService
	{
		Task<PagedResponse<IEnumerable<Winery>>> GetAllWineriesAsync(FetchRequest request);
		Task<Winery> GetWinerybyIdAsync(Guid id);
		Task<bool> AddWineryAsync(Winery winery);
		Task<bool> UpdateWineryAsync(Winery winery);
		Task<bool> RemoveWineryAsync(Guid id);
		Task<bool> WineryExistsAsync(Guid id);
	}
}
