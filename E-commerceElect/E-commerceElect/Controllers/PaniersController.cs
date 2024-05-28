using Microsoft.AspNetCore.Mvc;
using E_commerceElect.Models.Repositories;
using E_commerceElect.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace E_commerceElect.Controllers
{
    public class PaniersController : Controller
    {
        private readonly IRepositoryP<Panier> _panierRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<LignePanier> _lignePanierRepository;

        public PaniersController(IRepositoryP<Panier> panierRepository, UserManager<IdentityUser> userManager, IRepository<LignePanier> lignePanierRepository)
        {
            _panierRepository = panierRepository;
            _userManager = userManager;
            _lignePanierRepository = lignePanierRepository;
        }

        // GET: Paniers
        public IActionResult Index()
        {
            var paniers = _panierRepository.GetAll().Where(p => p.UserId == _userManager.GetUserId(User)).ToList();
            return View(paniers);
        }

        // GET: Paniers/Details/5
        public IActionResult Details(int id)
        {
            var panier = _panierRepository.Get(id);
            if (panier == null)
            {
                return NotFound();
            }
            return View(panier);
        }

        // GET: Paniers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Paniers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);

            var articleId = id;

            var lignePanier = new LignePanier
            {
                ArticleId = articleId,
            };

            Panier panier = await _panierRepository.GetAll().Where(p => p.UserId == user.Id).FirstOrDefaultAsync();

            if (panier == null)
            {
                panier = new Panier
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.Now
                };
                _panierRepository.Add(panier);
            }

            panier.LignePaniers.Add(lignePanier);
            _panierRepository.Update(panier);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterAuPanier(int articleId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // Retourner une erreur 401 Unauthorized si l'utilisateur n'est pas connecté
            }

            var user = await _userManager.GetUserAsync(User);

            var panier = await _panierRepository.GetAll()
                .Include(p => p.LignePaniers)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (panier == null)
            {
                panier = new Panier
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    LignePaniers = new List<LignePanier>()
                };
                _panierRepository.Add(panier);
            }

            var lignePanierExistante = panier.LignePaniers.FirstOrDefault(lp => lp.ArticleId == articleId);
            if (lignePanierExistante != null)
            {
                lignePanierExistante.Quantity++;
            }
            else
            {
                var nouvelleLignePanier = new LignePanier { ArticleId = articleId, Quantity = 1 };
                panier.LignePaniers.Add(nouvelleLignePanier);
            }

            _panierRepository.Update(panier);

            return RedirectToAction("Index", "Articles");
        }

        [HttpPost]
        public IActionResult IncrementerLignePanier(int lignePanierId)
        {
            var lignePanier = _lignePanierRepository.Get(lignePanierId);
            if (lignePanier == null)
            {
                return NotFound();
            }

            lignePanier.Quantity++;
            _lignePanierRepository.Update(lignePanier);

            return RedirectToAction(nameof(AfficherLignesCommande));
        }

        [HttpPost]
        public IActionResult DecrementerLignePanier(int lignePanierId)
        {
            var lignePanier = _lignePanierRepository.Get(lignePanierId);
            if (lignePanier == null)
            {
                return NotFound();
            }

            lignePanier.Quantity--;
            _lignePanierRepository.Update(lignePanier);

            return RedirectToAction(nameof(AfficherLignesCommande));
        }

        public async Task<IActionResult> AfficherLignesCommande()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); 
            }

            var user = await _userManager.GetUserAsync(User);

            var panier = await _panierRepository.GetAll()
                .Include(p => p.LignePaniers)
                .ThenInclude(lp => lp.Article)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (panier == null || panier.LignePaniers == null || !panier.LignePaniers.Any())
            {
                return View("PanierVide"); 
            }

            return View(panier.LignePaniers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SupprimerLignePanier(int lignePanierId)
        {
            var lignePanier = _lignePanierRepository.Get(lignePanierId);

            if (lignePanier == null)
            {
                return NotFound(); 
            }

            _lignePanierRepository.Delete(lignePanierId);

            return RedirectToAction(nameof(AfficherLignesCommande)); 
        }



        // GET: Paniers/Edit/5
        public IActionResult Edit(int id)
        {
            var panier = _panierRepository.Get(id);
            if (panier == null)
            {
                return NotFound();
            }
            return View(panier);
        }

        // POST: Paniers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,UserId,CreatedAt")] Panier panier)
        {
            if (id != panier.Id)
            {
                return NotFound();
            }

            
                try
                {
                    _panierRepository.Update(panier);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_panierRepository.Get(panier.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            return View(panier);
        }

        // GET: Paniers/Delete/5
        public IActionResult Delete(int id)
        {
            var panier = _panierRepository.Get(id);
            if (panier == null)
            {
                return NotFound();
            }

            return View(panier);
        }

        // POST: Paniers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _panierRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Commander(decimal totalPrix)
        {
            return RedirectToAction("ProcessPayment", "Paiement", new { montant = totalPrix });
        }

    }
}
