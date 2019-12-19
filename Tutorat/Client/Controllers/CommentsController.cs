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
    public class CommentsController : Controller
    {
        private readonly TutoratCoreContext _context;

        public CommentsController(TutoratCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetComments(int ServiceId)
        {
            // Récupère tous les commentaires liés à une offre.
            var comments = await _context.Comments.Where(c => c.ServiceId == ServiceId)
                                           .Include(c => c.Poster)
                                           .OrderByDescending(c => c.PostedDateTime)
                                           .ToListAsync();

            // afin de les lister sous l'offres 
            return PartialView("_Comments", comments);
        }

        
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var comments = await _context.Comments
        //        .Include(c => c.Poster)
        //        .Include(c => c.Service)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (comments == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(comments);
        //}

        //public IActionResult Create()
        //{
        //    ViewData["PosterId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
        //    ViewData["ServiceId"] = new SelectList(_context.Services, "IdentityKey", "Description");
        //    return View();
        //}

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PosterId,ServiceId,CommentText,PostedDateTime")] Comments comments)
        {
            // si le commentaire est valide mettre sur la date d'aujorud'hui et retourner à la page pour afficher les services
            comments.PostedDateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(comments);
                await _context.SaveChangesAsync();
                return RedirectToAction(controllerName: "Services", actionName: "Index");
            }
            ViewData["PosterId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comments.PosterId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "IdentityKey", "Description", comments.ServiceId);
            return RedirectToAction(controllerName: "Services", actionName:"Index");
        }

        //// GET: Comments/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var comments = await _context.Comments.FindAsync(id);
        //    if (comments == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["PosterId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comments.PosterId);
        //    ViewData["ServiceId"] = new SelectList(_context.Services, "IdentityKey", "Description", comments.ServiceId);
        //    return View(comments);
        //}

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,PosterId,ServiceId,CommentText,PostedDateTime")] Comments comments)
        //{
        //    if (id != comments.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(comments);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CommentsExists(comments.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PosterId"] = new SelectList(_context.AspNetUsers, "Id", "Id", comments.PosterId);
        //    ViewData["ServiceId"] = new SelectList(_context.Services, "IdentityKey", "Description", comments.ServiceId);
        //    return View(comments);
        //}

           
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Include(c => c.Poster)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comments == null)
            {
                return NotFound();
            }

            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentsExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
