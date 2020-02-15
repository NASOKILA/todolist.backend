namespace TodoList.Backend.Models.Interfaces
{
    public interface IUserEditViewModel
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }
    }
}
