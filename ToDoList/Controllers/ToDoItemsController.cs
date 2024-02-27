using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItemById(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var item = await _context.ToDosItems.FindAsync(id);
            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }
        [HttpGet("bytodos/{toDoID}")]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetItemsByToDoID(int toDoID)
        {
            if (toDoID == 0)
            {
                return BadRequest();
            }
            var items = await _context.ToDosItems.Where(x => x.ToDoId == toDoID).ToListAsync();
            if (items == null || items.Count == 0)
            {
                return NotFound();
            }
            return items;
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
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //HTTP Deletes
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var item = await _context.ToDosItems.FindAsync(id);
            if (item == null)
            {
                return BadRequest();
            }
            _context.ToDosItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
