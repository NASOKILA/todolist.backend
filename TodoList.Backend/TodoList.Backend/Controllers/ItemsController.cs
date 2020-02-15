using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoList.Backend.Models.Interfaces;
using TodoList.Backend.Models.ViewModels;

namespace TodoList.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly ILoggerService _logger;
        
        private readonly IItemRepository _itemRepository;

        private readonly IListRepository _listRepository;
        
        public ItemsController(ILoggerService logger, IItemRepository itemRepository, IListRepository listRepository)
        {
            _logger = logger;

            _itemRepository = itemRepository;

            _listRepository = listRepository;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _itemRepository.GetItemById(id);

            if (item == null) return BadRequest("Item does not exist");

            _logger.LogInfo($"Item with id {id} returned.");

            return Ok(item);
        }
        
        [HttpPost("add/{listId}")]
        public async Task<IActionResult> Post(int listId, [FromBody] ReceiveItemViewModel receiveItemViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var list = await _listRepository.GetListById(listId);

            if (list == null) return BadRequest("List does not exist.");
            
            await _itemRepository.AddAsync(receiveItemViewModel, list.Id);

            _itemRepository.CommitChanges();

            _logger.LogInfo($"New Item was created for list {list.Id}.");

            return Ok($"New Item was created for list {list.Id}.");
        }
        
        [HttpGet("shared")]
        public async Task<IActionResult> Get()
        {
            var sharedItems = await _itemRepository.GetSharedItems();
            
            _logger.LogInfo($"All shared items returned.");

            return Ok(sharedItems);
        }
        
        [HttpPut("toggle/share/{itemId}")]
        public async Task<IActionResult> Put(int itemId)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var item = await _itemRepository.GetItemById(itemId);

            if (item == null) return BadRequest("Item does not exist.");

            item.IsShared = !item.IsShared;
            
            await _itemRepository.UpdateAsync(item);

            _itemRepository.CommitChanges();

            string sharedStatus = !item.IsShared ? "Unshared" : "Shared";

            _logger.LogInfo($"Item {item.Id} for list {item.ListId} was {sharedStatus}.");

            return Ok($"Item {item.Description} for list {item.ListId} was {sharedStatus}.");
        }
        
        [HttpPut("toggle/complete/{itemId}")]
        public async Task<IActionResult> Complete(int itemId)
        {
            if (!ModelState.IsValid) return BadRequest();

            var item = await _itemRepository.GetItemById(itemId);

            if (item == null) return BadRequest("Item does not exist.");

            item.IsDone = !item.IsDone;

            await _itemRepository.UpdateAsync(item);

            _itemRepository.CommitChanges();

            string completedStatus = !item.IsDone? "New" : "Done";

            _logger.LogInfo($"Item {item.Id} for list {item.ListId}  was set to {completedStatus}.");

            return Ok($"Item {item.Description} for list {item.ListId} was set to {completedStatus}.");
        }
        
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemRepository.GetItemById(id);

            if (item == null) return BadRequest("Item does not exist");

            await _itemRepository.DeleteAsync(item);

            _itemRepository.CommitChanges();

            _logger.LogInfo($"Item {id} for list {item.ListId} was deleted.");

            return Ok($"Item {item.Description} for list {item.ListId} was deleted.");
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ReceiveItemViewModel receiveItemViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var item = await _itemRepository.GetItemById(id);

            if (item == null) return BadRequest("Item does not exist.");

            item.Description = receiveItemViewModel.Description;

            item.IsShared = receiveItemViewModel.IsShared;

            item.IsDone = receiveItemViewModel.IsDone;

            _itemRepository.CommitChanges();

            _logger.LogInfo($"Item {item.Description} was updated.");

            return Ok($"Item {item.Description} was updated.");
        }

    }
}