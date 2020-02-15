using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.ViewModels
{
    public class UserAdminViewModel : IUserAdminViewModel
    {
        public int Id { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}
