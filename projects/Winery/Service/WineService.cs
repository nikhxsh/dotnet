using WineryAPI.Mapper;
using WineryAPI.Models;
using WineryAPI.Storage.Models;
using WineryAPI.Storage.Repository;

namespace WineryAPI.Service
{
	public class WineService : IWineService
	{
		private readonly IRepository<WineDTO> _repository;

		public WineService(IRepository<WineDTO> repository)
		{
			_repository = repository;
		}

		public Task<PagedResponse<IEnumerable<Wine>>> GetAllWinesAsync(FetchRequest request)
		{
			try
			{
				var total = _repository.Count();
				var records = _repository.GetAll(request.Skip, request.Take);
				return Task.FromResult(new PagedResponse<IEnumerable<Wine>>
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

		public Task<Wine> GetWineByIdAsync(Guid id)
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

		public Task<bool> AddWineAsync(Wine wine)
		{
			try
			{
				var saved = _repository.Add(wine.ToDTO());
				return Task.FromResult(saved);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> UpdateWineAsync(Wine wine)
		{
			try
			{
				var updated = _repository.Update(wine.ToDTO());
				return Task.FromResult(updated);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> RemoveWineAsync(Guid id)
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

		public Task<bool> WineExistsAsync(Guid id)
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
