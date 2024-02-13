﻿using System.ComponentModel;

namespace ToDoList.DTOs
{
    public class NewToDoItemDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ToDoId { get; set; }
    }
}
