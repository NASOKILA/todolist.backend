using System.Collections.Generic;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Models.Interfaces
{
    public interface IUserViewModel
    {
        string UniqueToken { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }
        
        bool IsAdmin { get; set; }

        List<TodoListViewModel> Lists { get; set; }
    }
}
