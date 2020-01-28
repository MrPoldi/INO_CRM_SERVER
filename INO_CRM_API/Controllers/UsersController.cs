using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INO_CRM_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace INO_CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UsersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/Page/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("Page/{id}")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsersPage(int id, [FromQuery] string sMinDate, [FromQuery] string sMaxDate)
        {
            List<UserModel> users;
            DateTime minDate = DateTime.Parse(sMinDate);
            DateTime maxDate = DateTime.Parse(sMaxDate);
            if (minDate == null && maxDate == null)
            {
                users = await _context.Users.ToListAsync();
            }
            else
            {
                if (maxDate == null) maxDate = DateTime.Today;
                if (minDate == null) minDate = DateTime.Parse("01-01-1900");
                users = await _context.Users
                    .Where(u => !u.IsDeleted && u.dateOfBirth > minDate && u.dateOfBirth < maxDate).ToListAsync();
            }                     

            int count = users.Count;
            int pageSize = 10;
            int pageStart = (id - 1) * pageSize;

            if (pageStart + pageSize > count)
            {
                pageSize = count - pageStart;
            }

            return users.GetRange(pageStart, pageSize);
        }

        // GET: api/Users/Pages
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("Pages")]
        public async Task<ActionResult<int>> GetUsersPages([FromQuery] string sMinDate, [FromQuery] string sMaxDate)
        {
            int pageSize = 10;
            int count;
            DateTime minDate = DateTime.Parse(sMinDate);
            DateTime maxDate = DateTime.Parse(sMaxDate);

            if (minDate == null && maxDate == null)
            {
                count = await _context.Users.Where(u => !u.IsDeleted).CountAsync();
            }
            else
            {
                if (minDate == null) minDate = DateTime.Parse("01-01-1900");
                if (maxDate == null) maxDate = DateTime.Today;

                count = await _context.Users
                    .Where(u => !u.IsDeleted && u.dateOfBirth >= minDate && u.dateOfBirth <= maxDate).CountAsync();
            }

            
            int pages = (count / pageSize) + 1;


            return pages;
        }

        // GET: api/Users/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(int id)
        {
            var userModel = await _context.Users.FindAsync(id);

            if (userModel == null || userModel.IsDeleted)
            {
                return NotFound();
            }

            return userModel;
        }



        // GET: api/Users/5/roles
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("{id}/roles")]
        public async Task<ActionResult<RoleModel>> GetUserRole(int id)
        {
            //RoleModel userRole = _context.Roles.Find(_context.Users.Find(id).RoleId);            
            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null || userModel.IsDeleted)
            {
                return NotFound();
            }

            return userModel.Role;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(int id, UserModel userModel)
        {
            if (id != userModel.UserId)
            {
                return BadRequest();
            }

            if(userModel.Password == null)
            {
                userModel.Password = _context.Users.AsNoTracking().Where(u => u.UserId == userModel.UserId).Single().Password;
            }

            if(userModel.Role != null)
            {
                userModel.RoleId = _context.Roles.Where(r => r.Name == userModel.Role.Name).Single().RoleId;
                userModel.Role = null;
            }
            else
            {
                userModel.RoleId = _context.Users.AsNoTracking().Where(u => u.UserId == userModel.UserId).Single().RoleId;
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
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



        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {
            //userModel.Role = _context.Roles.Find(userModel.RoleId);
            userModel.IsDeleted = false;
            userModel.RoleId = 2; //Set role to user
            _context.Users.Add(userModel);            
            //_context.Users.Find(userModel).Role = _context.Roles.Find(userModel.RoleId);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = userModel.UserId }, userModel);
        }



        // DELETE: api/Users/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> DeleteUserModel(int id)
        {
            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            _context.Users.FindAsync(id).Result.IsDeleted = true;
            
            await _context.SaveChangesAsync();

            return userModel;
        }

        private bool UserModelExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
