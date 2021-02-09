using System.Linq;
using System.Threading.Tasks;
using library_test_task.Data.Models;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace library_test_task.Data
{
    public class RelationalRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbContext dbContext;
        private readonly DbSet<T> dbSet;

        public RelationalRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        /// <summary>
        /// Получить запись по идентификатору
        /// Может возвращать null если запись не найдена
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns>Запись</returns>
        public async Task<T> GetAsync(long id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Получить все записи сущности
        /// </summary>
        public IQueryable<T> GetAll()
        {
            return dbSet.AsQueryable();
        }

        /// <summary>
        /// Сохранить (добавить или обновить)
        /// </summary>
        public async Task<long> SaveAsync(T value)
        {
            if (value.Id == default)
            {
                await dbSet.AddAsync(value);
            }

            await dbContext.SaveChangesAsync();
            return value.Id;
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await dbSet.Where(x => x.Id == id).DeleteAsync();
        }
    }
}