using WineryAPI.Models;

namespace WineryAPI.Service
{
	public interface IWineService
	{
		Task<PagedResponse<IEnumerable<Wine>>> GetAllWinesAsync(FetchRequest request);
		Task<Wine> GetWineByIdAsync(Guid id);
		Task<bool> AddWineAsync(Wine wine);
		Task<bool> UpdateWineAsync(Wine wine);
		Task<bool> RemoveWineAsync(Guid id);
		Task<bool> WineExistsAsync(Guid id);
	}
}
