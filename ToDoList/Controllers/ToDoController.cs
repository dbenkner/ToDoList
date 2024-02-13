﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoListContext _context;
        public ToDoController(ToDoListContext context)
        {
            _context = context;
        }
        //GET api/ToDo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAllToDos ()
        {
            return await _context.ToDos.ToListAsync();
        }
        public async Task<ActionResult<ToDo>> ToDoDetails(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var result = await _context.ToDos.Include(x => x.Items).Where(x => x.Id == id).ToListAsync();   
            if (result.Count == 0 || Request == null) 
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Post api/ToDo
        [HttpPost]
        public async Task<ActionResult> NewToDo(ToDo toDo)
        {
            if (toDo == null)
            {
                return BadRequest();
            }
            _context.ToDos.Add(toDo);
            _context.SaveChanges();
            return Ok();    
        }
    }
}
