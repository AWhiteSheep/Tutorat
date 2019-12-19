using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Client.Data;
using Microsoft.AspNetCore.Identity;

namespace Client.Controllers
{
    public class HorrairesController : Controller
    {
        private readonly TutoratCoreContext _context;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SignInManager<AspNetUsers> _signInManager;

        public HorrairesController(TutoratCoreContext context, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // GET: Horraires
        public async Task<IActionResult> Index()
        {
            // envoie de tous les horraire
            var tutoratCoreContext = _context.Horraire
                .Include(h => h.Service);
            return View(await tutoratCoreContext.ToListAsync());
        }

        // GET: Horraires/Details/5
        public async Task<IActionResult> Students(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // elle retourne le user id
            var userId = _userManager.GetUserId(User);

            // retourner un liste des horaires reliées au service données en paramètre
            var service = _context.Services.Include(h => h.Horraire)
                .FirstOrDefault(v => v.TuteurId == userId && v.IdentityKey == id);


            // retourne vue non trouvé si null
            if (service == null)
            {
                return NotFound();
            }

            var horraire = await _context.Horraire
                .Include(h => h.Service)
                .FirstOrDefaultAsync(m => m.IdentityKey == id);


            if (horraire == null)
            {
                return NotFound();
            }
                      

            return View(horraire);
        }


        public IActionResult Create(int serviceId)
        {
            if (serviceId == null)
            {
                return NotFound();
            }


            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Login");
            }


            // elle retourne le user id
            var userId = _userManager.GetUserId(User);

            // retourner un liste des horaires reliées au service données en paramètre
            var service = _context.Services.Include(h => h.Horraire)
                .FirstOrDefault(v => v.TuteurId == userId && v.IdentityKey == serviceId);


            // retourne vue non trouvé si null
            if (service == null)
            {
                return NotFound();
            }

            // retourne un formulaire pour créer un plage pour le service donné en paramètre            
            ViewData["service"] = service;
            return View();
        }

        // POST: Horraires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdentityKey,ServiceId,Jour,HeureDebut,NbHeure,NbMinute,EleveMaxInscription")] Horraire horraire)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Login");
            }

            // elle retourne le user id
            var userId = _userManager.GetUserId(User);

            // retourner un liste des horaires reliées au service données en paramètre
            var service = _context.Services.Include(h => h.Horraire)
                .FirstOrDefault(v => v.TuteurId == userId && v.IdentityKey == horraire.ServiceId);


            // retourne vue non trouvé si null
            if (service == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(horraire);
                await _context.SaveChangesAsync();
                return Redirect("/Services/EditHoraire/" + horraire.ServiceId);
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "IdentityKey", "Description", horraire.ServiceId);
            return View(horraire);
        }

        // GET: Horraires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horraire = await _context.Horraire.FindAsync(id);
            if (horraire == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "IdentityKey", "Description", horraire.ServiceId);
            return View(horraire);
        }

        // POST: Horraires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdentityKey,ServiceId,Jour,HeureDebut,NbHeure,NbMinute,EleveMaxInscription")] Horraire horraire)
        {
            if (id != horraire.IdentityKey)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horraire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorraireExists(horraire.IdentityKey))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "IdentityKey", "Description", horraire.ServiceId);
            return View(horraire);
        }

        // GET: Horraires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horraire = await _context.Horraire
                .Include(h => h.Service)
                .FirstOrDefaultAsync(m => m.IdentityKey == id);
            if (horraire == null)
            {
                return NotFound();
            }

            return View(horraire);
        }

        // POST: Horraires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horraire = await _context.Horraire.FindAsync(id);
            _context.Horraire.Remove(horraire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorraireExists(int id)
        {
            return _context.Horraire.Any(e => e.IdentityKey == id);
        }
    }
}
