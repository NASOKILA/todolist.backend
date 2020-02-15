using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.Backend.Models.Interfaces;

namespace TodoList.Backend.Models.Database
{
    [Table("Lists")]
    public class TodoList : ITodoList, IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public List<TodoItem> Items { get; set; }
    }
}
