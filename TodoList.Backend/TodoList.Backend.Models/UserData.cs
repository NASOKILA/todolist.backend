using System.Collections.Generic;

namespace TodoList.Backend.Models
{
    public class UserData
    {
        public string Email { get; set; }

        public List<Database.TodoList> Lists { get; set; }

        public bool IsAdmin { get; set; }
    }
}
