using Microsoft.AspNetCore.Mvc;
using E_commerceElect.Models;
using E_commerceElect.Models.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Controllers
{
    public class LigneCommandesController : Controller
    {
        private readonly IRepository<LigneCommande> _repository;

        public LigneCommandesController(IRepository<LigneCommande> repository)
        {
            _repository = repository;
        }

        // GET: LigneCommandes
        public IActionResult Index()
        {
            var ligneCommandes = _repository.GetAll();
            return View(ligneCommandes);
        }

        // GET: LigneCommandes/Details/5
        public IActionResult Details(int id)
        {
            var ligneCommande = _repository.Get(id);
            if (ligneCommande == null)
            {
                return NotFound();
            }
            return View(ligneCommande);
        }

        // GET: LigneCommandes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LigneCommandes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CommandeId,ArticleId,Quantite,PrixUnitaire")] LigneCommande ligneCommande)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(ligneCommande);
                return RedirectToAction(nameof(Index));
            }
            return View(ligneCommande);
        }

        // GET: LigneCommandes/Edit/5
        public IActionResult Edit(int id)
        {
            var ligneCommande = _repository.Get(id);
            if (ligneCommande == null)
            {
                return NotFound();
            }
            return View(ligneCommande);
        }

        // POST: LigneCommandes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CommandeId,ArticleId,Quantite,PrixUnitaire")] LigneCommande ligneCommande)
        {
            if (id != ligneCommande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(ligneCommande);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.Get(ligneCommande.Id) == null)
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
            return View(ligneCommande);
        }

        // GET: LigneCommandes/Delete/5
        public IActionResult Delete(int id)
        {
            var ligneCommande = _repository.Get(id);
            if (ligneCommande == null)
            {
                return NotFound();
            }

            return View(ligneCommande);
        }

        // POST: LigneCommandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
