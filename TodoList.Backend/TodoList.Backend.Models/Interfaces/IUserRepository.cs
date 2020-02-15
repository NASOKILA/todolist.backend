using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Models.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailUniq(string email);
        
        Task<List<User>> GetAllUsers();

        Task<User> GetUserByUniqueToken(string uniqueToken);

        Task<User> GetUserById(int id);

        Task<User> GetUserByEmail(string email);

        Task AddAsync(UserViewModel user, string password);

        Task UpdateAsync(User user);
        
        Task DeleteAsync(string uniqueToken);

        void CommitChanges();
    }
}
