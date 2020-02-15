using System.Collections.Generic;

namespace TodoList.Backend.Models.Interfaces
{
    public interface IUser
    {
        string UniqueToken { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }

        string Password { get; set; }

        bool IsAdmin { get; set; }

        List<Database.TodoList> Lists { get; set; }
    }
}
