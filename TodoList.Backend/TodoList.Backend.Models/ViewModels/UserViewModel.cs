using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.ViewModels
{
    public class UserViewModel: IUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UniqueToken is required")]
        [StringLength(60, ErrorMessage = "UniqueToken can't be longer than 60 characters")]
        public string UniqueToken { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        public bool IsAdmin { get; set; }

        public List<TodoListViewModel> Lists { get; set; }
    }
}
