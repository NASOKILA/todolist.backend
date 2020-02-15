using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.Database
{
    [Table("Items")]
    public class TodoItem : ITodoItem, IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        
        public bool IsDone { get; set; }

        public TodoList List { get; set; }

        public int ListId { get; set; }
        
        public bool IsShared { get; set; }
    }
}
