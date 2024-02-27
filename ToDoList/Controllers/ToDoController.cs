using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.DTOs;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> ToDoDetails(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var result = await _context.ToDos.Include(x => x.Items).FirstOrDefaultAsync(x =>x.Id == id);
            if (result == null) 
            {
                return NotFound();
            }
            return result;
        }
        //GET api/ToDo/U/{id}
        [HttpGet("u/{uID}")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetByUserID(int uID)
        {
            if (uID == 0)
            {
                return BadRequest("invalid ID");
            }
            return await _context.ToDos.Where(T => T.UserId == uID).ToListAsync();
        }
        //Post api/ToDo
        [HttpPost]
        public async Task<ActionResult> NewToDo(NewToDoDTO newToDo)
        {
            if(newToDo == null)
            {
                return BadRequest();
            }
            ToDo toDo = new ToDo() {
                Name = newToDo.Name,
                Description = newToDo.Description,
                Priority = (PriorityLevel)newToDo.Priority,
                DateStarted = newToDo.DateStarted,
                UserId = newToDo.UserId
            };
            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
            return Ok(toDo);
        }
        //PUTs
        [HttpPut("comp/{id}")]
        public async Task<ActionResult<ToDo>> MarkComplete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var res = await _context.ToDos.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
            if (Request == null) {
                return BadRequest();
            }
            foreach(var item in res.Items)
            {
                if (item.isComplete == false) {
                    return BadRequest("All items must be marked completed first!");
                }
            }
            res.IsComplete = true;
            res.DateCompleted = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(res);
        }
        [HttpPut("setpri/{id}")]
        public async Task<ActionResult<ToDo>> SetPrioirtyLevel(int id, SetPrioityDTO setPrioityDTO)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var toDo = _context.ToDos.Find(id);
            if (toDo == null)
            {
                return BadRequest();
            }
            toDo.Priority = setPrioityDTO.PriorityLevel;
            await _context.SaveChangesAsync();
            return Ok(toDo);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(int id, ToDo updateToDo)
        {
            if (id != updateToDo.Id)
            {
                return BadRequest();
            }
            _context.Entry(updateToDo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!ToDoExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var toDo = await _context.ToDos.FindAsync(id);
            if (toDo == null) { return BadRequest(); }
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ToDoExists(int id)
        {
            return _context.ToDos.Any(x => x.Id == id);
        }
    }
}
