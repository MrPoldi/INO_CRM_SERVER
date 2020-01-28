using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INO_CRM_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace INO_CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RolesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleModel>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: api/Roles/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleModel>> GetRoleModel(int id)
        {
            var roleModel = await _context.Roles.FindAsync(id);

            if (roleModel == null)
            {
                return NotFound();
            }

            return roleModel;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleModel(int id, RoleModel roleModel)
        {
            if (id != roleModel.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(roleModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleModelExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<RoleModel>> PostRoleModel(RoleModel roleModel)
        {
            _context.Roles.Add(roleModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoleModel", new { id = roleModel.RoleId }, roleModel);
        }

        // DELETE: api/Roles/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoleModel>> DeleteRoleModel(int id)
        {
            var roleModel = await _context.Roles.FindAsync(id);
            if (roleModel == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(roleModel);
            await _context.SaveChangesAsync();

            return roleModel;
        }

        private bool RoleModelExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
