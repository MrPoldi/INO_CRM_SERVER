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
    public class CompaniesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CompaniesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyModel>> GetCompanyModel(int id)
        {
            var companyModel = await _context.Companies.FindAsync(id);

            if (companyModel == null)
            {
                return NotFound();
            }

            return companyModel;
        }

        // GET: api/Companies/Page/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("Page/{id}")]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> GetCompaniesPage(int id)
        {
            List<CompanyModel> companies = await _context.Companies.ToListAsync();

            int count = companies.Count;
            int pageSize = 10;
            int pageStart = (id - 1) * 10;

            if (pageStart + pageSize > count)
            {
                pageSize = count - pageStart;
            }

            return companies.GetRange(pageStart, pageSize);
        }

        // GET: api/Companies/Pages
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("Pages")]
        public async Task<ActionResult<int>> GetCompaniesPages()
        {
            int count = await _context.Companies.CountAsync();
            int pages = (count / 10) + 1;


            return pages;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyModel(int id, CompanyModel companyModel)
        {
            if (id != companyModel.CompanyId)
            {
                return BadRequest();
            }

            if (!BranchExists(companyModel.Branch.Name))
            {
                _context.Branches.Add(companyModel.Branch);
                await _context.SaveChangesAsync();
            }

            companyModel.BranchId = _context.Branches.Where(b => b.Name == companyModel.Branch.Name).Single().BranchId;
            companyModel.Branch = null;            
            companyModel.UserId = _context.Companies.AsNoTracking().Where(c => c.CompanyId == companyModel.CompanyId).Single().UserId;

            _context.Entry(companyModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyModelExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<ActionResult<CompanyModel>> PostCompanyModel(CompanyModel companyModel)
        {
            companyModel.IsDeleted = false;

            if (!BranchExists(companyModel.Branch.Name))
            {                
                _context.Branches.Add(companyModel.Branch);
                await _context.SaveChangesAsync();
            }            

            companyModel.BranchId = _context.Branches.Where(b => b.Name == companyModel.Branch.Name).Single().BranchId;
            companyModel.UserId = _context.Users.Where(u => u.Login == companyModel.User.Login).Single().UserId;
            companyModel.User = null;
            companyModel.Branch = null;
            _context.Companies.Add(companyModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompanyModel", new { id = companyModel.CompanyId }, companyModel);
        }

        // DELETE: api/Companies/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CompanyModel>> DeleteCompanyModel(int id)
        {
            var companyModel = await _context.Companies.FindAsync(id);
            if (companyModel == null)
            {
                return NotFound();
            }

            _context.Companies.FindAsync(id).Result.IsDeleted = true;
            await _context.SaveChangesAsync();

            return companyModel;
        }

        private bool CompanyModelExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }

        private bool BranchExists(string name)
        {
            return _context.Branches.Any(b => b.Name == name);
        }        
    }
}
