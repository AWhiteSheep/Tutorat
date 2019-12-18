using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Client.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Exchange.WebServices.Data;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    public class ServicesController : Controller
    {
        private readonly TutoratCoreContext _context;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SignInManager<AspNetUsers> _signInManager;

        public ServicesController(TutoratCoreContext context, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
        [Authorize]
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


                await _context.Entry(application).Reference(a => a.IdentifiantUtilisateurNavigation)
                                            .LoadAsync();

                await _context.Entry(application).Reference(a => a.IdentifiantHoraireNavigation)
                                                .LoadAsync();

                await _context.Entry(application.IdentifiantHoraireNavigation)
                                    .Reference(h => h.Service)
                                    .LoadAsync();

                SendApplicationEmail(application);

                return RedirectToAction(nameof(Index));
            }

            return Problem();
        }

        private void SendApplicationEmail(Demandes application)
        {
            ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            myservice.Credentials = new WebCredentials();

            string message = $"Confirmation d'inscription -- {application.IdentifiantHoraireNavigation.Service.ServiceTypeCode}; {System.Environment.NewLine}";

            string emailDestination = application.IdentifiantUtilisateurNavigation.Email;

            try
            {
                string serviceUrl = "https://outlook.office365.com/ews/exchange.asmx";

                myservice.Url = new Uri(serviceUrl);
                EmailMessage emailMessage = new EmailMessage(myservice);
                emailMessage.Subject = "Test Subject";
                emailMessage.Body = new MessageBody(message);

                emailMessage.ToRecipients.Add(emailDestination);
                emailMessage.Send();
                Console.Write("message envoyé");
                Console.ReadKey();
            }
            catch (SmtpException exception)
            {
                string msg = "Erreur:message n'a pas pu être envoyé";
                msg += exception.Message;
                throw new Exception(msg);
            }

            catch (AutodiscoverRemoteException exception)
            {
                string msg = "Erreur:message n'a pas pu être envoyé";
                msg += exception.Message;
                throw new Exception(msg);

            }
        }

            // GET: Services/Details/5
            public async Task<IActionResult> EditHoraire(int? id)
        {
            if (id == null)
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
            var services = _context.Services.Include(h => h.Horraire)
                .FirstOrDefault(v => v.TuteurId == userId && v.IdentityKey == id);

            if (services == null)
            {
                return NotFound();
            }

            return View(services);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["TuteurId"] = _userManager.GetUserId(User);
                return View();
            }
            else 
            {
                return Redirect("/Home");
            }
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
