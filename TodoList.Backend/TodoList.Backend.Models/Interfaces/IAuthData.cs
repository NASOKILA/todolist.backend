namespace TodoList.Backend.Models.Interfaces
{
    public interface IAuthData
    {
        string Token { get; set; }

        long TokenExpirationTime { get; set; }

        string UniqueToken { get; set; }
    }
}
