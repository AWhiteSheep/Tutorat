using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Client.Data;
using Microsoft.AspNetCore.Identity;
using Client.Data.Authorize;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    [Authorize]
    public class SuperAdminController : Controller
    {
        private readonly TutoratCoreContext context;
        private readonly RoleManager<IdentityRole> _roleManager;

        private UserManager<AspNetUsers> _userManager;

        public SuperAdminController(TutoratCoreContext context, RoleManager<IdentityRole> roleManager, UserManager<AspNetUsers> userManager)
        {
            this.context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: SuperAdmin
        public async Task<IActionResult> Index()
        {
            // return les asp net users
            ViewData["Users"] = _userManager.Users.ToList();
            // donnes tous les roles pour les associées et les lister
            return View(await _roleManager.Roles.ToListAsync()); 
        }

        public class input {
            public string userId { get; set; }
            public string roleId { get; set; }
        }

        // GET: SuperAdmin
        [HttpPost]
        public async Task<IActionResult> Relate([Bind("userId,roleId")] input input)
        {
            await _userManager.AddToRoleAsync(await _userManager.GetUserAsync(User), 
                (await _roleManager.FindByIdAsync(input.roleId)).Name);

            // return les asp net users
            ViewData["Users"] = _userManager.Users.ToList();
            // donnes tous les roles pour les associées et les lister
            return View("Index", await _roleManager.Roles.ToListAsync()); 
        }

        // GET: SuperAdmin/Details/5
        public async Task<IActionResult> Roles()
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var roles = await _context.Roles
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (roles == null)
            //{
            //    return NotFound();
            //}

            //return View(roles);
            return View();
        }

        // GET: SuperAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SuperAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Roles roles)
        {
            if (ModelState.IsValid)
            {
                var roleNew = new IdentityRole(roles.Name);
                await _roleManager.CreateAsync(roleNew);
                return RedirectToAction(nameof(Index));
            }
            return View(roles);
        }

        // GET: SuperAdmin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var roles = await _context.Roles.FindAsync(id);
            //if (roles == null)
            //{
            //    return NotFound();
            //}
            //return View(roles);
            return View();
        }

        // POST: SuperAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Roles roles)
        {
            //if (id != roles.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(roles);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!RolesExists(roles.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(roles);
            return View();
        }

        // GET: SuperAdmin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var roles = await _context.Roles
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (roles == null)
            //{
            //    return NotFound();
            //}

            //return View(roles);
            return View();
        }

        // POST: SuperAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var roles = await _context.Roles.FindAsync(id);
            //_context.Roles.Remove(roles);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return View();
        }

        private bool RolesExists(string id)
        {
            //return _context.Roles.Any(e => e.Id == id);
            return false;
        }
    }
}
