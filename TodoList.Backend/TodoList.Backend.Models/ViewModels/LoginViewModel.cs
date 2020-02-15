using System.ComponentModel.DataAnnotations;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
