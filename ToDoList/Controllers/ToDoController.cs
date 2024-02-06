using Microsoft.AspNetCore.Mvc;
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
