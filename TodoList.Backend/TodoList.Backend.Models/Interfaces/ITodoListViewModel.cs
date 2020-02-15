using System.Collections.Generic;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Models.Interfaces
{
    public interface ITodoListViewModel
    {
        string Title { get; set; }

        List<TodoItemViewModel> Items { get; set; }
    }
}
