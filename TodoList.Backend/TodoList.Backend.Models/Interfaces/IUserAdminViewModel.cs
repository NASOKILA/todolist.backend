namespace TodoList.Backend.Models.Interfaces
{
    public interface IUserAdminViewModel
    {
        int Id { get; set; }

        bool IsAdmin { get; set; }
    }
}
