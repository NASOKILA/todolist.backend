using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Data;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;
using static TodoList.Backend.Utils.Repositories.BaseRepository;

namespace TodoList.Backend.Utils.Repositories
{
    public class ListRepository : EntityBaseRepository<Models.Database.TodoList>, IListRepository
    {
        public ListRepository(TodoListDbContext context) : base(context) { }

        public async Task<List<Models.Database.TodoList>> GetAllListsAsync()
        {
            List<Models.Database.TodoList> allLists = await this.GetAllListsFromDb();

            return allLists;
        }

        public async Task<Models.Database.TodoList> GetListById(int id)
        {
            var list = await this.GetSingleListById(id);

            return list;
        }

        public async Task<Models.Database.TodoList> GetListByTitle(string title)
        {
            var list = await this.GetSingleListByTitle(title);

            return list;
        }
        
        public async Task AddAsync(string title, int userId)
        {
            Models.Database.TodoList newList = new Models.Database.TodoList()
            {
                Title = title,
                Items = new List<TodoItem>(),
                UserId = userId
            };

            await this.Add(newList);
        }
        
        public async Task UpdateAsync(int id, TodoListViewModel todoList)
        {
            var list = await this.GetListById(id);

            list.Title = todoList.Title;

            await this.Update(list);
        }

        public async Task DeleteAsync(Models.Database.TodoList list)
        {
            await this.Delete(list);
        }

        public void CommitChanges()
        {
            this.Commit();
        }
    }
}
