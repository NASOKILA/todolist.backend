using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Backend.Models.Database
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surename { get; set; }

        public string Email { get; set; }

        public List<List> Lists { get; set; }
    }
}
