using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Models.Interfaces
{
    public interface IListRepository
    {
        Task<List<Database.TodoList>> GetAllListsAsync();

        Task<Database.TodoList> GetListByTitle(string title);

        Task<Database.TodoList> GetListById(int id);
        
        Task AddAsync(string title, int userId);

        Task UpdateAsync(int id, TodoListViewModel todoList);
        
        Task DeleteAsync(Database.TodoList list);
        
        void CommitChanges();
    }
}
