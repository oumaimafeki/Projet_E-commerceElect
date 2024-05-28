using Microsoft.AspNetCore.Mvc;
using E_commerceElect.Models;
using E_commerceElect.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using Microsoft.Extensions.Options;

namespace E_commerceElect.Controllers
{
    public class PaiementController : Controller
    {
        private readonly IRepositoryP<Panier> _panierRepository;
        private readonly IRepositoryC<Commande> _commandeRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly StripeSettings _stripeSettings;

        public PaiementController(
            IRepositoryP<Panier> panierRepository,
            IRepositoryC<Commande> commandeRepository,
            UserManager<IdentityUser> userManager,
            IOptions<StripeSettings> stripeSettings)
        {
            _panierRepository = panierRepository;
            _commandeRepository = commandeRepository;
            _userManager = userManager;
            _stripeSettings = stripeSettings.Value;
        }

        public async Task<IActionResult> ProcessPayment(decimal montant)
        {
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error"); 
            }

            var panier = _panierRepository.GetAll()
                .Include(p => p.LignePaniers)
                .ThenInclude(lp => lp.Article) 
                .FirstOrDefault(p => p.UserId == user.Id);

            

            if (panier == null || panier.LignePaniers == null || !panier.LignePaniers.Any())
            {
                return View("PanierVide"); 
            }

            var ligneCommandes = panier.LignePaniers
                .Where(lp => lp.Article != null)
                .Select(lp => new LigneCommande
                {
                    ArticleId = lp.ArticleId,
                    Quantite = lp.Quantity,
                    PrixUnitaire = lp.Article.Prix
                }).ToList();

            if (!ligneCommandes.Any())
            {
                return View("Error"); 
            }

            var commande = new Commande
            {
                UserId = user.Id,
                DateCommande = DateTime.Now,
                MontantTotal = montant,
                LignesCommande = ligneCommandes
            };

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(montant * 100), 
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            if (paymentIntent.Status == "succeeded")
            {
                commande.EstReglee = 1; 
            }

            _commandeRepository.Add(commande);
            _commandeRepository.SaveChanges(); 

            _panierRepository.Delete(panier.Id);
            _panierRepository.SaveChanges(); 



            return RedirectToAction("Confirmation", new { montant, clientSecret = paymentIntent.ClientSecret });
        }


        public IActionResult Confirmation(decimal montant, string clientSecret)
        {
            ViewBag.Montant = montant;
            ViewBag.ClientSecret = clientSecret;
            ViewBag.PublishableKey = _stripeSettings.PublishableKey;
            return View();
        }
    }
}
