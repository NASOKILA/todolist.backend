using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Models.Database;

namespace TodoList.Backend.Models.Interfaces
{
    //Basic methods to work with entities
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<List<T>>GetAll();

        int Count();

        Task<User> GetSingleUserByUniqueToken(string token);

        Task<User> GetSingleUserByEmail(string token);

        Task<Database.TodoList> GetSingleListById(int id);

        Task<TodoItem> GetSingleItemById(int id);

        Task<Database.TodoList> GetSingleListByTitle(string title);

        Task<TodoItem> GetSingleItemByDescription(string description);

        Task Add(T entity);

        Task Update(T entity);

        Task Delete(T entity);
        
        void Commit();
    }
}
