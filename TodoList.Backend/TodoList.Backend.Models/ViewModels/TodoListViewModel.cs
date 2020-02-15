using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.ViewModels
{
    public class TodoListViewModel : ITodoListViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public List<TodoItemViewModel> Items { get; set; }
    }
}
