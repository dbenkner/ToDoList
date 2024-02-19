using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoListContext _context;
        public ToDoItemsController(ToDoListContext context)
        {
            _context = context;
        }
        //HTTP GETS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAllItems()
        {
            return await _context.ToDosItems.ToListAsync();
        }

        //HTTP POSTs
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> AddToDoItem(NewToDoItemDTO toDoItemDTO)
        {
            if (toDoItemDTO == null)
            {
                return BadRequest();
            }
            ToDoItem toDoItem = new ToDoItem() { 
                Name = toDoItemDTO.Name,
                Description = toDoItemDTO.Description,
                isComplete = false,
                ToDoId = toDoItemDTO.ToDoId
            };
            await _context.ToDosItems.AddAsync(toDoItem);
            await _context.SaveChangesAsync();
            return Ok(toDoItem);
        }
        //HTTP Puts
        [HttpPut("comp/{id}")]
        public async Task<ActionResult<ToDoItem>> MarkItemComplete(int id)
        {
            if(id == 0) {
                return BadRequest();
            }
            var item = await _context.ToDosItems.FindAsync(id);
            item.isComplete = true;
            await _context.SaveChangesAsync();
            return Ok(item);
        }
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditToDoItem(int id, ToDoItem toDoItem)
        {
            if (id == 0 || id != toDoItem.Id) { 
            return BadRequest();
            }
            _context.Entry(toDoItem).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
