using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Models;
using TaskApi.DTOs;

namespace TaskApi.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase{
        private readonly AppDbContext _context;

        public UserController(AppDbContext context){
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
            return CreatedAtAction(nameof(GetById), new {id = user.Id}, user);
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
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}