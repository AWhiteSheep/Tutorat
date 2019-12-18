using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Client.Models;
using Microsoft.EntityFrameworkCore;
using Client.Data;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TutoratCoreContext context;

        public HomeController(ILogger<HomeController> logger, TutoratCoreContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index(string id)
        {
            // pour afficher tous les catégories qui sont existantes
            ViewData["Categories"] = context.Categories.ToArray();

            // donne un filter pour filter la recherche
            if (!string.IsNullOrEmpty(id))
            {
                List<Services> services = new List<Services>();
                var servicescategories = context.ServiceCategorie.Include(e => e.Service).Where(d => d.Category.Name == id).ToList();           
                foreach (var union in servicescategories)
                {
                    services.Add(union.Service);
                }
                ViewData["ActiveCategory"] = id;
                ViewData["Services"] = services;

                return View();
            }

            // si pas filter affiche tous les services
            ViewData["Services"] = context.Services.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
