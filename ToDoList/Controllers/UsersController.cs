using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ToDoListContext _context;

        public UsersController(ToDoListContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //Post: api/Users/registeruser
        [HttpPost("registeruser")]
        public async Task<ActionResult<User>> RegisterUser(RegisterUserDTO registerUser)
        {
            if (await UserExists(registerUser.Username))
            {
                return BadRequest("Username Already Exists!");
            }
            using var hmac = new HMACSHA512();
            User user = new User()
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                UserName = registerUser.Username.ToLower(),
                isAdmin = registerUser.isAdmin,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        [HttpPost("loginuser")]
        public async Task<ActionResult<User>> LogInUser(LogInUserDTO logInUser)
        
        {
            var user  = _context.Users.SingleOrDefault(u => u.UserName == logInUser.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Login");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logInUser.Password));
            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Login");
                }
            }
            return Ok(user);
        }
        [HttpPost("NewPW")]
        public async Task<ActionResult<User>> NewPW(NewPasswordDTO newPasswordDTO)
        {
            var res = await _context.Users.FirstOrDefaultAsync(U => U.UserName == newPasswordDTO.Username.ToLower());
            if (res == null)
            {
                return Unauthorized("Invalid Login");
            }
            using var hmac = new HMACSHA512(res.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPasswordDTO.OldPassword));
            for( int i = 0;i < computedHash.Length; i++)
            {
                if (computedHash[i] != res.PasswordHash[i])
                {
                    return Unauthorized("Invalid Login");
                }
            }
            res.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPasswordDTO.NewPassword));
            res.PasswordSalt = hmac.Key;
            await _context.SaveChangesAsync();
            return Ok(res);
        }
        
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(U => U.UserName == username.ToLower());  
        }
    }
}
