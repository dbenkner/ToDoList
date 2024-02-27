namespace ToDoList.DTOs
{
    public class NewToDoDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateStarted { get; set; } = DateTime.Now;
        public int Priority { get; set; } = 1;
        public int UserId { get; set; }
    }
}
