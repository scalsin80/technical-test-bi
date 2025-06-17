using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.VisualBasic;

namespace APIUser.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly dbdataContext _context;

        public UsersController(ILogger<UsersController> logger, dbdataContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("Call to api/Users");

            var idUser = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

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
            if (id != user.idUser)
            {
                return BadRequest();
            }

            try
            {
                User userDb = await _context.Users.FindAsync(id);
                if(userDb == null)
                {
                    return BadRequest("Validate data send");
                }

                //Move
                userDb.name = user.name;
                userDb.password = user.password;
                userDb.address = user.address;
                userDb.birthday = user.birthday;
                userDb.updated = user.updated;
                userDb.idUserUpdated = id;
                userDb.enabled = user.enabled;

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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace.ToString());

                return NoContent();
            }
            
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                //Move
                user.registered = DateTime.Now;
                user.updated = DateTime.Now;


                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.idUser }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace.ToString());

                return NoContent();
            }
            
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (!user.canEdited)
                {
                    return BadRequest("Cant change some main users");
                }

                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
            
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.idUser == id);
        }
    }
}
