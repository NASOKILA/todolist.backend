using System.ComponentModel.DataAnnotations;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.ViewModels
{
    public class ListReceivedViewModel : IListReceivedViewModel
    {
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }
    }
}
