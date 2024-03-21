using WineryAPI.Mapper;
using WineryAPI.Models;
using WineryAPI.Storage.Models;
using WineryAPI.Storage.Repository;

namespace WineryAPI.Service
{
	public class WineryService : IWineryService
	{
		private readonly IRepository<WineryDTO> _repository;

		public WineryService(IRepository<WineryDTO> repository)
		{
			_repository = repository;
		}

		public Task<PagedResponse<IEnumerable<Winery>>> GetAllWineriesAsync(FetchRequest request)
		{
			try
			{
				var total = _repository.Count();
				var records = _repository.GetAll(request.Skip, request.Take);
				return Task.FromResult(new PagedResponse<IEnumerable<Winery>>
				{
					Total = total,
					FilteredTotal = records.Count(),
					Result = records.ToModels()
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<Winery> GetWinerybyIdAsync(Guid id)
		{
			try
			{
				var record = _repository.GetById(id);
				return Task.FromResult(record.ToModel());
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> AddWineryAsync(Winery winery)
		{
			try
			{
				var saved = _repository.Add(winery.ToDTO());
				return Task.FromResult(saved);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> UpdateWineryAsync(Winery winery)
		{
			try
			{
				var updated = _repository.Update(winery.ToDTO());
				return Task.FromResult(updated);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> RemoveWineryAsync(Guid id)
		{
			try
			{
				var deleted = _repository.Delete(id);
				return Task.FromResult(deleted);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> WineryExistsAsync(Guid id)
		{
			try
			{
				var exists = _repository.Exists(id);
				return Task.FromResult(exists);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
