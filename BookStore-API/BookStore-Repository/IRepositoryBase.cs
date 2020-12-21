using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IList<T>> FindAll();

        Task<bool> IsExist(int Id);

        Task<T> FindByID(int Id);

        Task<bool> Create(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(T entity);

        Task<bool> Save();
    }
}