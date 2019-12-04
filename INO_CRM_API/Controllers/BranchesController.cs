using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INO_CRM_API.Models;

namespace INO_CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public BranchesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/branches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchModel>>> GetBranches()
        {
            return await _context.Branches.ToListAsync();
        }

        // GET: api/branches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchModel>> GetBranchModel(int id)
        {
            var branchModel = await _context.Branches.FindAsync(id);

            if (branchModel == null)
            {
                return NotFound();
            }

            return branchModel;
        }

        // PUT: api/branches/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranchModel(int id, BranchModel branchModel)
        {
            if (id != branchModel.BranchId)
            {
                return BadRequest();
            }

            _context.Entry(branchModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchModelExists(id))
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

        // POST: api/branches
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<BranchModel>> PostBranchModel(BranchModel branchModel)
        {
            _context.Branches.Add(branchModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBranchModel", new { id = branchModel.BranchId }, branchModel);
        }

        // DELETE: api/branches/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BranchModel>> DeleteBranchModel(int id)
        {
            var branchModel = await _context.Branches.FindAsync(id);
            if (branchModel == null)
            {
                return NotFound();
            }

            _context.Branches.Remove(branchModel);
            await _context.SaveChangesAsync();

            return branchModel;
        }

        private bool BranchModelExists(int id)
        {
            return _context.Branches.Any(e => e.BranchId == id);
        }
    }
}
