using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Backend.Data;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Utils.Repositories
{
    public class BaseRepository
    {
        public class EntityBaseRepository<T> : IEntityBaseRepository<T>
            where T : class, IEntityBase, new()
        {
            private TodoListDbContext _context;

            public EntityBaseRepository(TodoListDbContext context)
            {
                _context = context;
            }

            public virtual async Task<List<T>> GetAll()
            {
                return await _context.Set<T>().ToListAsync();
            }
            
            public virtual int Count()
            {
                return _context.Set<T>().Count();
            }

            public async Task<List<Models.Database.TodoList>> GetAllListsFromDb()
            {
                return await _context.Lists
                    .Include(list => list.Items)
                    .Include(list => list.User)
                    .ToListAsync();
            }

            public async Task<List<User>> GetAllUsersFromDb()
            {
                return await _context.Users
                    .Include(user => user.Lists)
                    .ThenInclude(list => list.Items)
                    .ToListAsync();
            }
            
            public async Task<User> GetUserByIdFromDb(int id)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }

            public async Task<User> GetSingleUserByUniqueToken(string token)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UniqueToken == token);
            }

            public async Task<User> GetSingleUserByEmail(string email)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            
            public async Task<Models.Database.TodoList> GetSingleListById(int id)
            {
                return await _context.Lists.FirstOrDefaultAsync(u => u.Id == id);
            }

            public async Task<Models.Database.TodoList> GetSingleListByTitle(string title)
            {
                return await _context.Lists.FirstOrDefaultAsync(u => u.Title == title);
            }
            
            public async Task<TodoItem> GetSingleItemById(int id)
            {
                return await _context.Items.FirstOrDefaultAsync(u => u.Id == id);
            }
            
            public async Task<TodoItem> GetSingleItemByDescription(string description)
            {
                return await _context.Items.FirstOrDefaultAsync(u => u.Description == description);
            }

            public virtual async Task Add(T entity)
            {
                EntityEntry dbEntityEntry = _context.Entry<T>(entity);
                 _context.Set<T>().Add(entity);
            }
            
            public virtual async Task Update(T entity)
            {
                EntityEntry dbEntityEntry = _context.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Modified;
            }

            public virtual async Task Delete(T entity)
            {
                EntityEntry dbEntityEntry = _context.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            
            public virtual void Commit()
            {
                _context.SaveChanges();
            }
        }
    }
}
