namespace TodoList.Backend.Models.Interfaces
{
    public interface IAuthService
    {
        AuthData GetAuthData(string id);

        bool VerifyPassword(string modelPassword, string userPassword);

        string HashPassword(string password);
    }
}
