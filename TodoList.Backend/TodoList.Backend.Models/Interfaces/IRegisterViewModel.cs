namespace TodoList.Backend.Models.Interfaces
{
    public interface IRegisterViewModel
    {
        string FirstName { get; set; }
        
        string LastName { get; set; }
        
        string Email { get; set; }
        
        string Password { get; set; }

        bool IsAdmin { get; set; }
    }
}
