using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDoList.Models
{
    public enum PriorityLevel
    {
        Low,
        Medium,
        High,
        Critical
    }
    public class ToDo
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4080)]
        public string? Description { get; set; }
        public DateTime DateStarted { get; set; } = DateTime.Now;
        public DateTime? DateCompleted { get; set; }
        public bool IsComplete { get; set; } = false;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        public int UserId { get; set; }
        public virtual List<ToDoItem>? Items { get; set; }
    }
}
