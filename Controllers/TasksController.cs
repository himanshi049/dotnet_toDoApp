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
        public async Task<IActionResult> GetAll(
             [FromQuery] int page = 1,
             [FromQuery] int pageSize = 10,
             [FromQuery] int? userId = null,
             [FromQuery] bool? isCompleted = null) 
        
        {
            // Start with the base query
            var query = _context.Tasks.AsNoTracking().AsQueryable();

            // Filter by userId if provided
            if(userId.HasValue)
                query = query.Where(t => t.UserId == userId.Value);

            // Filter by completion status if provided
            if(isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            // Validate pagination parameters
            if(page < 1) page = 1;
            if(pageSize < 1) pageSize = 10;
            if(pageSize > 100) pageSize = 100;

            // Calculate how many items to skip
            int skip = (page - 1) * pageSize;

            // Apply pagination and projection to DTO
            var tasks = await query
                .OrderBy(t => t.Id)
                .Skip(skip)
                .Take(pageSize)
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
