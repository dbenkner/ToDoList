﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ToDoListContext : DbContext
    {
        public ToDoListContext (DbContextOptions<ToDoListContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoList.Models.User> Users { get; set; } = default!;
        public DbSet<ToDoList.Models.ToDo> ToDos { get; set; } = default!;
        public DbSet<ToDoList.Models.ToDoItem> ToDosItems { get; set; } = default!;
    }
}
