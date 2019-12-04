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
    public class TradeNotesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public TradeNotesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/TradeNotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeNoteModel>>> GetTradeNotes()
        {
            return await _context.TradeNotes.ToListAsync();
        }

        // GET: api/TradeNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TradeNoteModel>> GetTradeNoteModel(int id)
        {
            var tradeNoteModel = await _context.TradeNotes.FindAsync(id);

            if (tradeNoteModel == null)
            {
                return NotFound();
            }

            return tradeNoteModel;
        }

        // PUT: api/TradeNotes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTradeNoteModel(int id, TradeNoteModel tradeNoteModel)
        {
            if (id != tradeNoteModel.NoteId)
            {
                return BadRequest();
            }

            _context.Entry(tradeNoteModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeNoteModelExists(id))
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

        // POST: api/TradeNotes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TradeNoteModel>> PostTradeNoteModel(TradeNoteModel tradeNoteModel)
        {
            _context.TradeNotes.Add(tradeNoteModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTradeNoteModel", new { id = tradeNoteModel.NoteId }, tradeNoteModel);
        }

        // DELETE: api/TradeNotes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TradeNoteModel>> DeleteTradeNoteModel(int id)
        {
            var tradeNoteModel = await _context.TradeNotes.FindAsync(id);
            if (tradeNoteModel == null)
            {
                return NotFound();
            }

            _context.TradeNotes.Remove(tradeNoteModel);
            await _context.SaveChangesAsync();

            return tradeNoteModel;
        }

        private bool TradeNoteModelExists(int id)
        {
            return _context.TradeNotes.Any(e => e.NoteId == id);
        }
    }
}
