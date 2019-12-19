using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Client.Data;

namespace Client.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly TutoratCoreContext _context;

        public CategoriesController(TutoratCoreContext context)
        {
            _context = context;
        }

        // affiche la liste des catégories possibles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        ////
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var categories = await _context.Categories
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (categories == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(categories);
        //}

            // retourne la vue pour créer un nouvelle catégorie
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Categories categories)
        {
            // valid la nouvelle catégorie 
            if (ModelState.IsValid)
            {
                // ajoute à la base de donnée et retourne une action de redirection
                _context.Add(categories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categories);
        }

        // affiche afin de faire l'édition de la catégori re trouvé
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Categories categories)
        {

            // si la catégorie détient un id, continuons la transaction
            if (id != categories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // update la catégorie avec tous les entités qui pourrait suivre cette dernière et faire le updates
                    _context.Update(categories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.Id))
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
            return View(categories);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            // supprime la catégorie
            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
