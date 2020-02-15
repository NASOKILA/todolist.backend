using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListsController : ControllerBase
    {
        private readonly ILoggerService _logger;
        
        private readonly IListRepository _listRepository;

        private readonly IUserRepository _userRepository;

        public ListsController(ILoggerService logger, IListRepository listRepository, IUserRepository userRepository)
        {
            _logger = logger;
            
            _listRepository = listRepository;

            _userRepository = userRepository;
        }
        
        [HttpGet("mylists/{uniqueToken}")]
        public async Task<IActionResult> Get(string uniqueToken)
        {   
            var allLists = await this._listRepository.GetAllListsAsync();

            List<Models.Database.TodoList> myLists = allLists.Where(list => list.User.UniqueToken == uniqueToken).ToList();

            _logger.LogInfo($"Lists for user {uniqueToken} returned.");

            return Ok(myLists);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var list = await _listRepository.GetListById(id);

            if (list == null) return BadRequest("List does not exist.");
            
            _logger.LogInfo($"List {id} returned.");

            return Ok(list);
        }
        
        [HttpPost("{uniqueToken}")] 
        public async Task<IActionResult> Post(string uniqueToken, [FromBody] ListReceivedViewModel listReceivedViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var user = await _userRepository.GetUserByUniqueToken(uniqueToken);

            if (user == null) return BadRequest("User does not exist.");

            await _listRepository.AddAsync(listReceivedViewModel.Title, user.Id);

            _listRepository.CommitChanges();

            _logger.LogInfo($"New list was created.");

            return Ok("New list was created.");
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ListReceivedViewModel listReceivedViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var list = await _listRepository.GetListById(id);

            if (list == null) return BadRequest("List does not exist.");

            list.Title = listReceivedViewModel.Title;

            _listRepository.CommitChanges();

            _logger.LogInfo($"List {id} was updated.");

            return Ok($"List {listReceivedViewModel.Title} was updated.");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var list = await _listRepository.GetListById(id);

            if (list == null) return BadRequest("List does not exist");

            await _listRepository.DeleteAsync(list);

            _listRepository.CommitChanges();
            
            _logger.LogInfo($"List {id} for user {list.UserId} was deleted.");

            return Ok($"List {list.Title} was deleted.");
        }
    }
}
