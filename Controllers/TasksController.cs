using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.DTOs;
using TaskApi.Models;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _context.Tasks
                .Include(t => t.User)
                .AsNoTracking()
                .Select(t => new TaskWithUserDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    UserId = t.UserId,
                    UserName = t.User.Name
                })
                .ToListAsync();
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.User)
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TaskWithUserDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    UserId = t.UserId,
                    UserName = t.User.Name
                })
                .FirstOrDefaultAsync();
            
            if (task == null)
                return NotFound();
            
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            if(! userExists)
                return BadRequest("User does not exist.");
            
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                UserId = dto.UserId ?? 0
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.Title = dto.Title;
            task.Description = dto.Description;
            
            await _context.SaveChangesAsync();
            return Ok(task);
        }

        // PATCH: api/tasks/{id}/toggle
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
