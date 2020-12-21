using BookStore_DomainModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_Repository
{
    public interface IAuthorRepository : IRepositoryBase<Author>
    {
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext db;

        public AuthorRepository(DataContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(Author entity)
        {
            await db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            db.Authors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Author>> FindAll()
        {
            IList<Author> authors = await db.Authors.ToListAsync();
            return authors;
        }

        public async Task<Author> FindByID(int Id)
        {
            Author author = await db.Authors.Where(find => find.Id == Id).FirstOrDefaultAsync();
            return author;
        }

        public async Task<bool> IsExist(int Id)
        {
            var exist = await db.Authors.Where(find => find.Id == Id).FirstOrDefaultAsync();
            if (exist != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Save()
        {
            var changes = await db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Author entity)
        {
            db.Authors.Update(entity);
            return await Save();
        }
    }
}