namespace TodoList.Backend.Models.Interfaces
{
    public interface ITodoItem
    {
        string Description { get; set; }

        bool IsDone { get; set; }

        bool IsShared { get; set; }
    }
}
