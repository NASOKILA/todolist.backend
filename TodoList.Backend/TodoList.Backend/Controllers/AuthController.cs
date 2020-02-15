using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Backend.Models;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly IUserRepository _userRepository;

        private readonly ILoggerService _logger;

        public AuthController(IAuthService authService, IUserRepository userRepository, ILoggerService logger)
        {
            _authService = authService;

            _userRepository = userRepository;

            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepository.GetUserByEmail(model.Email);

            if (user == null)
            {
                return BadRequest("No user with this email");
            }

            var passwordValid = _authService.VerifyPassword(model.Password, user.Password);

            if (!passwordValid)
            {
                return BadRequest("Invalid password");
            }

            AuthData authData = _authService.GetAuthData(user.UniqueToken);

            UserData userData = new UserData()
            {
                Email = user.Email,
                Lists = new List<Models.Database.TodoList>(),
                IsAdmin = user.IsAdmin
            };

            _logger.LogInfo($"User {user.Email} logged in.");

            var result = new UserDetailsAuthData() { AuthData = authData, UserData = userData };

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailUniq = await _userRepository.IsEmailUniq(model.Email);

            if (!emailUniq) return BadRequest("User with this email already exists");
            
            var uniqueToken = Guid.NewGuid().ToString();

            var user = new UserViewModel
            {
                UniqueToken = uniqueToken,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Lists = new List<TodoListViewModel>(),
                IsAdmin = model.IsAdmin
            };

            await _userRepository.AddAsync(user, model.Password);

            _userRepository.CommitChanges();

            AuthData authData = _authService.GetAuthData(user.UniqueToken);

            UserData userData = new UserData()
            {
                Email = user.Email,
                Lists = new List<Models.Database.TodoList>(),
                IsAdmin = user.IsAdmin
            };

            _logger.LogInfo($"User {user.Email} registered.");

            var result = new UserDetailsAuthData() { AuthData = authData, UserData = userData };

            return Ok(result);
        }
    }
}
