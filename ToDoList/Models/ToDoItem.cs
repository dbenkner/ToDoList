using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        [StringLength(60)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Description { get; set; }
        public bool isComplete { get; set; } = false;
        public int ToDoId { get; set; }
        public ToDo? ToDo { get; set; }
    }
}
