using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Backend.Data;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;
using static TodoList.Backend.Utils.Repositories.BaseRepository;

namespace TodoList.Backend.Utils.Repositories
{
    public class ItemRepository : EntityBaseRepository<TodoItem>, IItemRepository
    {
        public ItemRepository(TodoListDbContext context) : base(context) { }

        public async Task AddAsync(ReceiveItemViewModel receiveItemViewModel, int listId)
        {
            TodoItem todoItem = new TodoItem()
            {
                Description = receiveItemViewModel.Description,
                IsDone = receiveItemViewModel.IsDone,
                IsShared = receiveItemViewModel.IsShared,
                ListId = listId
            };

            await this.Add(todoItem);
        }

        public void CommitChanges()
        {
            this.Commit();
        }
        
        public async Task DeleteAsync(TodoItem todoItem)
        {
            await this.Delete(todoItem);
        }
        
        public async Task<TodoItem> GetItemById(int id)
        {
            var item = await this.GetSingleItemById(id);

            return item;
        }

        public async Task<List<TodoItem>> GetSharedItems()
        {
            var sharedItems = await this.GetAll();

            return sharedItems.Where(i => i.IsShared == true).ToList();
        }


        public async Task UpdateAsync(TodoItem todoItem)
        {
            await this.Update(todoItem);
        }
    }
}
