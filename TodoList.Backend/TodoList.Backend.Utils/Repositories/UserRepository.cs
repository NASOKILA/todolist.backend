using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Data;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;

using static TodoList.Backend.Utils.Repositories.BaseRepository;

namespace TodoList.Backend.Utils.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        IAuthService _authService;
        
        public UserRepository(IAuthService authService, TodoListDbContext context) : base(context)
        {
            _authService = authService;
        }

        public async Task<bool> IsEmailUniq(string email)
        {
            var user = await this.GetSingleUserByEmail(email);

            return user == null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await this.GetAllUsersFromDb();

            return users;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await this.GetUserByIdFromDb(id);

            return user;
        }
        
        public async Task<User> GetUserByUniqueToken(string uniqueToken)
        {
            var user = await this.GetSingleUserByUniqueToken(uniqueToken);

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await this.GetSingleUserByEmail(email);

            return user;
        }

        public async Task AddAsync(UserViewModel user, string password)
        {
            User newUser = new User()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _authService.HashPassword(password),
                UniqueToken = user.UniqueToken,
                IsAdmin = user.IsAdmin,
                Lists = new List<Models.Database.TodoList>()
            };

            await this.Add(newUser);
        }

        public async Task UpdateAsync(User user)
        {
            await this.Update(user);
        }
        
        public async Task DeleteAsync(string uniqueToken)
        {
            User currentUser = await this.GetUserByUniqueToken(uniqueToken);
            await this.Delete(currentUser);
        }

        public void CommitChanges() {
            this.Commit();
        }
    }
}
