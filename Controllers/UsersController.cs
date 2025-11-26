using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Models;
using TaskApi.DTOs;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO dto)
        {
            var user = new User
            {
                Name = dto.Name
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users
     .AsNoTracking()
     .Where(u => u.Id == id)
     .Select(u => new UserWithTasksDto
     {
         Id = u.Id,
         Name = u.Name,
         Tasks = u.Tasks.Select(t => new TaskDto
         {
             Id = t.Id,
             Title = t.Title,
             Description = t.Description,
             IsCompleted = t.IsCompleted,
             CreatedAt = t.CreatedAt
         }).ToList()
     })
     .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}