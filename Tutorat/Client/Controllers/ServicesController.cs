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
    public class ServicesController : Controller
    {
        private readonly TutoratCoreContext _context;
        private readonly UserManager<AspNetUsers> _userManager;

        public ServicesController(
            TutoratCoreContext context,
            UserManager<AspNetUsers> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            // envoie à la vue les tuteurs ou tutrices et les horraires étant associé(e)s
            var tutoratCoreContext = _context.Services
                    .Include(s => s.Tuteur)
                    .Include(h => h.Horraire);
            return View(await tutoratCoreContext.ToListAsync());
        }

        public async Task<IActionResult> FilteredServices(int Typecode)
        {
            var services = await _context.Services.Where(s => s.ServiceTypeCode == Typecode)
                                                    .ToListAsync();

            return PartialView("_FilteredServices", services);
        }

        // GET: Services/Horaire/5
        public async Task<IActionResult> Horaire(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // recherche du services demandé et retourne les horaires en liens
            var services = await _context.Services
                .Include(s => s.Tuteur)
                .Include(h => h.Horraire)
                .ThenInclude(d => d.Demandes)
                .FirstOrDefaultAsync(m => m.IdentityKey == id);            

            if (services == null)
            {
                return NotFound();
            }

            // envoit d'une booleen si l'utilisateur connecté est le propriétaire ou non
            string UserId = _userManager.GetUserId(User);
            ViewBag.IsTutor = UserId == services.TuteurId;
            ViewBag.UserId = UserId;

            // revoit de la partiel
            return PartialView("_Horaire", services);
        }

        // POST: Services/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply([Bind("IdentifiantUtilisateur,IdentifiantHoraire")] Demandes application)
        {
            // rapporte le premier horaire afin de vérifier si l'application est vraisemblable
            var horaireServices = await _context.Horraire
                .FirstOrDefaultAsync(m => m.IdentityKey == application.IdentifiantHoraire);

            // vérifie si l'horaire de l'inscription existe dans la base de donnée
            if (horaireServices == null)
            {
                return NotFound();
            }

            // si oui ajoute l'application si valide
            if (ModelState.IsValid)
            {
                // ajoute l'application
                _context.Add(application);
                // sauvegarde et redirige
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return Problem();
        }

        // GET: Services/Details/5
        public async Task<IActionResult> EditHoraire(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services
                .Include(s => s.Tuteur)
                .Include(h => h.Horraire)
                .FirstOrDefaultAsync(m => m.IdentityKey == id);
            if (services == null)
            {
                return NotFound();
            }

            List<Comments> Comments = await _context.Comments.Where(c => c.ServiceId == id)
                                                                .ToListAsync();

            ViewBag.Comments = Comments;

            return PartialView("_Horaire", services);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            ViewData["TuteurId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdentityKey,TuteurId,Titre,Description")] Services services)
        {
            if (ModelState.IsValid)
            {
                _context.Add(services);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TuteurId"] = new SelectList(_context.AspNetUsers, "Id", "Id", services.TuteurId);
            return View(services);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services.FindAsync(id);
            if (services == null)
            {
                return NotFound();
            }
            ViewData["TuteurId"] = new SelectList(_context.AspNetUsers, "Id", "Id", services.TuteurId);
            return View(services);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdentityKey,TuteurId,Titre,Description")] Services services)
        {
            if (id != services.IdentityKey)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(services);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesExists(services.IdentityKey))
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
            ViewData["TuteurId"] = new SelectList(_context.AspNetUsers, "Id", "Id", services.TuteurId);
            return View(services);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services
                .Include(s => s.Tuteur)
                .FirstOrDefaultAsync(m => m.IdentityKey == id);
            if (services == null)
            {
                return NotFound();
            }

            return View(services);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var services = await _context.Services.FindAsync(id);
            _context.Services.Remove(services);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicesExists(int id)
        {
            return _context.Services.Any(e => e.IdentityKey == id);
        }
    }
}
