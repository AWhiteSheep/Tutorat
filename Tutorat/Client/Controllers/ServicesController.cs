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
using Newtonsoft.Json;
using Suave.Utils;
using Microsoft.Extensions.Primitives;

namespace Client.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly TutoratCoreContext _context;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SignInManager<AspNetUsers> _signInManager;
        private bool CurrentIsAdmin { get; set; }

        public ServicesController(TutoratCoreContext context, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // GET: Services
        [AllowAnonymous]
        public async Task<IActionResult> Index(string message)
        {            
            message = ControllerContext.HttpContext.Request.Query.Keys.FirstOrDefault();
            // envoie à la vue les tuteurs ou tutrices et les horraires étant associé(e)s
            var tutoratCoreContext = _context.Services
                    .Include(s => s.Tuteur)
                    .Include(h => h.Horraire);
            if (!string.IsNullOrEmpty(message))
                ViewData["success"] = System.Net.WebUtility.UrlDecode(message);
            return View(await tutoratCoreContext.ToListAsync());
        }

        public async Task<IActionResult> FilteredServices(int Typecode)
        {
            var services = await _context.Services.Where(s => s.ServiceTypeCode == Typecode)
                                                    .ToListAsync();

            return PartialView("_FilteredServices", services);
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FirstOrDefaultAsync(s => s.IdentityKey == id);

            if (service == null)
            {
                return NotFound();
            }

            return View("Details", service);
        }

        // GET: Services/Horaire/5
        [AllowAnonymous]
        // permet la vue de l'horaire même déconnecté
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
            if (!string.IsNullOrEmpty(UserId))
            {
                ViewBag.IsTutor = UserId == services.TuteurId;
                ViewBag.UserId = UserId;
            }

            // revoit de la partiel
            return PartialView("_Horaire", services);
        }

        // POST: Services/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply([Bind("IdentifiantUtilisateur,IdentifiantHoraire")] Demandes application)
        {
            string message = new string("");
            // rapporte le premier horaire afin de vérifier si l'application est vraisemblable
            var horaireServices = _context.Horraire
                .First(m => m.IdentityKey == application.IdentifiantHoraire);        
            // vérifie si l'horaire de l'inscription existe dans la base de donnée
            if (horaireServices == null)
            {
                return NotFound();
            }

            // si oui ajoute l'application si valide
            if (ModelState.IsValid)
            {
                // présrit la nouvelle application
                application.DateCreated = DateTime.Now;
                application.IdentifiantHoraire = horaireServices.IdentityKey;

                if (_context.Demandes.Any(e => e.IdentifiantHoraire == application.IdentifiantHoraire && e.IdentifiantUtilisateur == application.IdentifiantUtilisateur))
                {
                    message = "Vous êtes déjà inscrit.";
                    return Redirect("/Services/Index/?"+ System.Net.WebUtility.UrlEncode(message));
                }
                
                Console.WriteLine("Adding: " + application.IdentifiantUtilisateur);
                Console.WriteLine("TO: " + application.IdentifiantHoraire);

                try
                {

                    // ajoute l'application
                    _context.Demandes.Add(application);

                    // savegarde les changement 
                    _context.SaveChanges();
                }
                catch {
                    message = "Vous participez déjà  a un service qui à la même plage horaire, ou il y a un conflit d'horaire.";
                    // une erreur survient si à la même heure il est inscrit à fait la demande pour la même heure
                    return Redirect("/Services/Index/?" + System.Net.WebUtility.UrlEncode(message));
                }

                //SendApplicationEmail(application);

                message = "Vous êtes maintenant inscrit!";
                return Redirect("/Services/Index/?" + System.Net.WebUtility.UrlEncode(message));
            }

            return Problem();
        }


        // configuration de l'email de configuration en recevant la demande 
        // afin d'envoyer à la personne une confirmation d'inscription
        [Authorize]
        private void SendApplicationEmail(Demandes application)
        {
            // initilisation du service exchange donner par le service nugget par microsoft
            ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            myservice.Credentials = new WebCredentials();

            // préparation du message à envoyer à la personne qui à fait la demande
            string message = $"Confirmation d'inscription -- {application.IdentifiantHoraireNavigation.Service.ServiceTypeCode}; {System.Environment.NewLine}";

            // email du destinataire donc la personne qui a fait la demande dont nous la retrouvont à travers la navigation
            // de la demande 
            string emailDestination = application.IdentifiantUtilisateurNavigation.Email;

            try
            {
                // url où le service nous permet de nous authentifier avant d'envoyer l'email
                string serviceUrl = "https://outlook.office365.com/ews/exchange.asmx";

                // préparation de l'objet message afin de tout bien envelopper avant l'envoie
                myservice.Url = new Uri(serviceUrl);
                EmailMessage emailMessage = new EmailMessage(myservice);
                emailMessage.Subject = "Test Subject";
                emailMessage.Body = new MessageBody(message);

                // ajout du récipient lequel est le nouveau venu
                emailMessage.ToRecipients.Add(emailDestination);
                emailMessage.Send();
                // message envoyé!
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

        /// <summary>
        /// Édition de l'horaire donnée pour le service requis
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> EditHoraire(int? id)
        {
            if (id == null)
            {
                return NotFound();
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

            // retourne la vue avec le service et une référence à l'objet horraire au travers l'entité service
            return View(services);
        }

        [Authorize]
        public IActionResult Create()
        {
            // Retourne la vue pour créer un nouveau service
            ViewData["TuteurId"] = _userManager.GetUserId(User);
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("IdentityKey,TuteurId,Titre,Description")] Services services)
        {
            // vérifie si le service, est complet et ajoute à la base de données
            if (ModelState.IsValid)
            {
                _context.Add(services);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Retourne la vue pour créer continuer la création, car elle n'est pas terminé si elle n'est pas valide
            ViewData["TuteurId"] = _userManager.GetUserId(User);
            return View(services);
        }

        // au click de la page index pour faire la modification du service
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
            // Retourne la vue pour créer continuer la création
            var user = await _userManager.GetUserAsync(User);
            ViewData["UserName"] = user.Email;
            ViewData["UserId"] = user.Id;
            return View(services);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("IdentityKey,TuteurId,Titre,Description")] Services services)
        {
            if (id != services.IdentityKey)
            {
                return NotFound();
            }



            // retourne l'utilisateur 
            var user = await _userManager.GetUserAsync(User);
            CurrentIsAdmin = await  _userManager.IsInRoleAsync(user, "Admin");

            if (ModelState.IsValid)
            {

                // si il n'est pas admin faire la vérification que le service lui appartient
                if(services.TuteurId != user.Id)
                    if(!CurrentIsAdmin)
                           return NotFound(); // ne pas continuer l'opération
                try
                {
                    // sinon faire le update du nouveau service
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
            // Retourne la vue pour créer continuer la création
            ViewData["TuteurId"] = _userManager.GetUserId(User);
            return View(services);
        }

        [Authorize]
        // envoit une demande de confirmation pour la suppression du service demadé
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            // essait de retourner le service qui appartien au tuteur demandé
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
            
            
            
            // retourne l'utilisateur 
            var user = await _userManager.GetUserAsync(User);
            CurrentIsAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            // si il n'est pas admin faire la vérification que le service lui appartient
            if (services.TuteurId != user.Id)
                if (!CurrentIsAdmin)
                    return NotFound(); // ne pas continuer l'opération

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
