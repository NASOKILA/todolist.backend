using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Backend.Models.Database
{
    public class List
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Item> Items { get; set; }
    }
}
