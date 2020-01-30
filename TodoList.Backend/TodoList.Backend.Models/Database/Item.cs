using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Backend.Models.Database
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Description { get; set; }

        public enum Status { New=1, Done=2 }

        public bool IsShared { get; set; }
    }
}
