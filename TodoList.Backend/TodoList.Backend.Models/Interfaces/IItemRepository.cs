using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Models.Interfaces
{
    public interface IItemRepository
    {
        Task<TodoItem> GetItemById(int id);

        Task AddAsync(ReceiveItemViewModel receiveItemViewModel, int listId);

        Task UpdateAsync(TodoItem todoItem);
        
        Task DeleteAsync(TodoItem todoItem);

        Task<List<TodoItem>> GetSharedItems();

        void CommitChanges();
    }
}
