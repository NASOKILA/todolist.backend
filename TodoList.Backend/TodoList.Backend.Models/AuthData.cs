using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models
{
    //ViewModel the user will receive after authentication
    public class AuthData : IAuthData
    {
        public string Token { get; set; }

        public long TokenExpirationTime { get; set; }

        public string UniqueToken { get; set; }
    }
}
