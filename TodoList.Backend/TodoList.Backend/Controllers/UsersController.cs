using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Models.Database;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerService _logger;

        private readonly IUserRepository _userRepository;

        public UsersController(ILoggerService logger, IUserRepository userRepository)
        {
            _logger = logger;

            _userRepository = userRepository;
        }

        // GET api/users
        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var usersFromDb = await _userRepository.GetAllUsers();

            List<UserViewModel> users = new List<UserViewModel>();

            foreach (User user in usersFromDb)
            {
                List<TodoListViewModel> userLists = new List<TodoListViewModel>();

                if (user.Lists != null)
                {
                    foreach (var list in user.Lists)
                    {
                        List<TodoItemViewModel> listItems = new List<TodoItemViewModel>();

                        if (list.Items != null)
                        {
                            foreach (TodoItem item in list.Items)
                            {
                                TodoItemViewModel newItem = new TodoItemViewModel()
                                {
                                    Id = item.Id,
                                    IsDone = item.IsDone,
                                    Description = item.Description,
                                    IsShared = item.IsShared
                                };

                                listItems.Add(newItem);
                            }
                        }

                        userLists.Add(new TodoListViewModel()
                        {
                            Items = listItems,
                            Title = list.Title,
                            Id = list.Id
                        });
                    }
                }

                users.Add(new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAdmin = user.IsAdmin,
                    Lists = userLists,
                    UniqueToken = user.UniqueToken
                });
            }

            _logger.LogInfo($"All users returned.");

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {   
            User userFromDb = await _userRepository.GetUserById(id);
            
            if (userFromDb == null) return BadRequest("User does not exist.");
            
            UserEditViewModel userEditData = new UserEditViewModel() {
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                Email = userFromDb.Email
            };
            
            _logger.LogInfo($"User with id {id} returned.");

            return Ok(userEditData);
        }
        
        // PUT api/users/5
        [HttpPut("toggle/admin")]
        public async Task<IActionResult> Put([FromBody] UserAdminViewModel userAdminView)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userRepository.GetUserById(userAdminView.Id);

            if (user == null) return BadRequest("User does not exist.");

            user.IsAdmin = !userAdminView.IsAdmin;

            await _userRepository.UpdateAsync(user);

            _userRepository.CommitChanges();

            string admin = user.IsAdmin ? "Admin" : "User";

            _logger.LogInfo($"User with id {user.Id} set to {admin}.");

            return Ok($"{user.FirstName} set to {admin} successfully.");
        }

        // DELETE api/users/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null) return BadRequest("User does not exist.");

            await _userRepository.DeleteAsync(user.UniqueToken);

            _userRepository.CommitChanges();

            _logger.LogInfo($"User with id {user.Id} deleted.");

            return Ok($"{user.FirstName} was deleted successfully.");
        }
        
        // PUT api/users/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserEditViewModel userViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userRepository.GetUserById(id);

            if (user == null) return BadRequest("User does not exist.");

            user.FirstName = userViewModel.FirstName;

            user.LastName= userViewModel.LastName;

            user.Email = userViewModel.Email;

            await _userRepository.UpdateAsync(user);

            _userRepository.CommitChanges();

            _logger.LogInfo($"User with {id} was updated.");

            return Ok($"{user.FirstName} was updated successfully.");
        }
    }
}