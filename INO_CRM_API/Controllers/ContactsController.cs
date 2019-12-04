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
    public class ContactsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ContactsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactPersonModel>>> GetContactPeople()
        {
            return await _context.ContactPeople.ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactPersonModel>> GetContactPersonModel(int id)
        {
            var contactPersonModel = await _context.ContactPeople.FindAsync(id);

            if (contactPersonModel == null)
            {
                return NotFound();
            }

            return contactPersonModel;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactPersonModel(int id, ContactPersonModel contactPersonModel)
        {
            if (id != contactPersonModel.ContactId)
            {
                return BadRequest();
            }

            _context.Entry(contactPersonModel).State = EntityState.Modified;

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
        [HttpPost]
        public async Task<ActionResult<ContactPersonModel>> PostContactPersonModel(ContactPersonModel contactPersonModel)
        {
            _context.ContactPeople.Add(contactPersonModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactPersonModel", new { id = contactPersonModel.ContactId }, contactPersonModel);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactPersonModel>> DeleteContactPersonModel(int id)
        {
            var contactPersonModel = await _context.ContactPeople.FindAsync(id);
            if (contactPersonModel == null)
            {
                return NotFound();
            }

            _context.ContactPeople.Remove(contactPersonModel);
            await _context.SaveChangesAsync();

            return contactPersonModel;
        }

        private bool ContactPersonModelExists(int id)
        {
            return _context.ContactPeople.Any(e => e.ContactId == id);
        }
    }
}
