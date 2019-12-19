using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Client.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    [Authorize]
    public class CommunicationsController : Controller
    {
        private readonly TutoratCoreContext _context;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SignInManager<AspNetUsers> _signInManager;

        // injection du user manager et le sign in afin de faire les vérification et retourner les bonnes données
        public CommunicationsController(TutoratCoreContext context, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult GetCommunication()
        {
            // retourne tous les communications lié à l'utilisateur connecté
            var userId = _userManager.GetUserId(User);

            // fetch de la base de données tous les communication lié avec l'utilisateur 
            var communications = _context.Communication.Include(e => e.FromUserNavigation)
                .Include(d => d.SendToNavigation).Where(e => e.FromUser == userId || e.SendTo == userId).ToList();
                       
            // retourne tous les communications
            return View(communications);
        }

        // GET: api/Communications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Communication>> GetCommunication(string id)
        {
            var communication = await _context.Communication.FindAsync(id);

            if (communication == null)
            {
                return NotFound();
            }

            return communication;
        }

        // PUT: api/Communications/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunication(string id, Communication communication)
        {
            if (id != communication.Id)
            {
                return BadRequest();
            }

            _context.Entry(communication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunicationExists(id))
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

        // POST: api/Communications
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Communication>> PostCommunication(Communication communication)
        {
            _context.Communication.Add(communication);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CommunicationExists(communication.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCommunication", new { id = communication.Id }, communication);
        }

        // DELETE: api/Communications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Communication>> DeleteCommunication(string id)
        {
            var communication = await _context.Communication.FindAsync(id);
            if (communication == null)
            {
                return NotFound();
            }

            _context.Communication.Remove(communication);
            await _context.SaveChangesAsync();

            return communication;
        }

        private bool CommunicationExists(string id)
        {
            return _context.Communication.Any(e => e.Id == id);
        }
    }
}
