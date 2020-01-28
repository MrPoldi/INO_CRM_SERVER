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
    public class ContactsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ContactsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactPersonModel>>> GetContactPeople()
        {
            return await _context.ContactPeople.ToListAsync();
        }

        // GET: api/Contacts/Company/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("Company/{id}")]
        public async Task<ActionResult<IEnumerable<ContactPersonModel>>> GetContactPeopleForCompany(int id, [FromQuery] string searchName)
        {
            return await _context.ContactPeople.Where(c => !c.IsDeleted 
                                                      && c.CompanyId == id 
                                                      && (searchName != null ? c.LastName.StartsWith(searchName) : true))
                                                      .ToListAsync();
        }

        // GET: api/Contacts/5
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactPersonModel>> GetContactPersonModel(int id)
        {
            var contactPersonModel = await _context.ContactPeople.FindAsync(id);

            if (contactPersonModel == null || contactPersonModel.IsDeleted)
            {
                return NotFound();
            }

            return contactPersonModel;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactPersonModel(int id, ContactPersonModel contact)
        {
            if (id != contact.ContactId)
            {
                return BadRequest();
            }

            contact.CompanyId = _context.Companies.Where(c => c.Name == contact.Company.Name).Single().CompanyId;
            contact.Company = null;
            contact.UserId = _context.ContactPeople.AsNoTracking().Where(c => c.ContactId == contact.ContactId).Single().UserId;
            
            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactPersonModelExists(id))
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

        // POST: api/Contacts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<ActionResult<ContactPersonModel>> PostContactPersonModel(ContactPersonModel contact)
        {
            contact.IsDeleted = false;
            contact.UserId = _context.Users.Where(u => u.Login == contact.User.Login).Single().UserId;
            contact.User = null;

            _context.ContactPeople.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactPersonModel", new { id = contact.ContactId }, contact);
        }

        // DELETE: api/Contacts/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactPersonModel>> DeleteContactPersonModel(int id)
        {
            var contactPersonModel = await _context.ContactPeople.FindAsync(id);
            if (contactPersonModel == null)
            {
                return NotFound();
            }

            _context.ContactPeople.FindAsync(id).Result.IsDeleted = true;
            await _context.SaveChangesAsync();

            return contactPersonModel;
        }

        private bool ContactPersonModelExists(int id)
        {
            return _context.ContactPeople.Any(e => e.ContactId == id);
        }
    }
}
