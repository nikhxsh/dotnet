using WineryAPI.Models;

namespace WineryAPI.Storage.Repository
{
    public interface IRepository<T>
	{
        int Count();
		bool Exists(Guid id);
		IEnumerable<T> GetAll(int skip, int take);
		T GetById(Guid id);
		bool Add(T entity);
		bool Update(T entity);
        bool Delete(Guid id);
    }
}