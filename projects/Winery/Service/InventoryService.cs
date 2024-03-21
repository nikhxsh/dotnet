using WineryAPI.Models;
using WineryAPI.Storage.Models;
using WineryAPI.Storage.Repository;

namespace WineryAPI.Service
{
	public class InventoryService : IInventoryService
	{
		private readonly IRepository<WineDTO> _wineRepository;
		private readonly IRepository<WineryDTO> _wineryRepository;

		public InventoryService(
			IRepository<WineDTO> wineRepository, 
			IRepository<WineryDTO> wineryRepository
			)
		{
			_wineRepository = wineRepository;
			_wineryRepository = wineryRepository;
		}

		public Task<Wine> FetchWineAsync(Guid wineryId, Guid wineId)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResponse<IEnumerable<Wine>>> ListWinesAsync(Guid wineryId, FetchRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
