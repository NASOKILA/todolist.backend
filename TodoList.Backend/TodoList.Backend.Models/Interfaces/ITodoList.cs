using System.Collections.Generic;
namespace TodoList.Backend.Models.Interfaces
{
    public interface ITodoList
    {
        string Title { get; set; }

        List<Database.TodoItem> Items { get; set; }
    }
}
