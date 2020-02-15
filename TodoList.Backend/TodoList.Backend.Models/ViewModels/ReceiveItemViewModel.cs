using System.ComponentModel.DataAnnotations;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.ViewModels
{
    public class ReceiveItemViewModel : IReceiveItemViewModel
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        
        public bool IsDone { get; set; }
        
        public bool IsShared { get; set; }
    }
}
