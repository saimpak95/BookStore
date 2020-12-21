using BookStore_DomainModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_Repository
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
    }

    public class BookRepository : IBookRepository
    {
        private readonly DataContext db;

        public BookRepository(DataContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(Book entity)
        {
            await db.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            db.Books.Remove(entity);
            return await Save();
        }

        public async Task<IList<Book>> FindAll()
        {
            IList<Book> books = await db.Books.ToListAsync();
            return books;
        }

        public async Task<Book> FindByID(int Id)
        {
            Book book = await db.Books.Where(find => find.Id == Id).FirstOrDefaultAsync();
            return book;
        }

        public async Task<bool> IsExist(int Id)
        {
            var exist = await db.Books.Where(find => find.Id == Id).FirstOrDefaultAsync();
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

        public async Task<bool> Update(Book entity)
        {
            db.Books.Update(entity);
            return await Save();
        }
    }
}