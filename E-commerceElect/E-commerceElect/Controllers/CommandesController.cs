using Microsoft.AspNetCore.Mvc;
using E_commerceElect.Models;
using E_commerceElect.Models.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Controllers
{
    public class CommandesController : Controller
    {
        private readonly IRepository<Commande> _repository;

        public CommandesController(IRepository<Commande> repository)
        {
            _repository = repository;
        }

        // GET: Commandes
        public IActionResult Index()
        {
            var commandes = _repository.GetAll();
            return View(commandes);
        }

        // GET: Commandes/Details/5
        public IActionResult Details(int id)
        {
            var commande = _repository.Get(id);
            if (commande == null)
            {
                return NotFound();
            }
            return View(commande);
        }

        // GET: Commandes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Commandes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,UtilisateurId,DateCommande,MontantTotal,EstReglee")] Commande commande)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(commande);
                return RedirectToAction(nameof(Index));
            }
            return View(commande);
        }

        // GET: Commandes/Edit/5
        public IActionResult Edit(int id)
        {
            var commande = _repository.Get(id);
            if (commande == null)
            {
                return NotFound();
            }
            return View(commande);
        }

        // POST: Commandes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,UtilisateurId,DateCommande,MontantTotal,EstReglee")] Commande commande)
        {
            if (id != commande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(commande);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.Get(commande.Id) == null)
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
            return View(commande);
        }

        // GET: Commandes/Delete/5
        public IActionResult Delete(int id)
        {
            var commande = _repository.Get(id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: Commandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
