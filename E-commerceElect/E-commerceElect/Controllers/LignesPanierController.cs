using Microsoft.AspNetCore.Mvc;
using E_commerceElect.Models;
using E_commerceElect.Models.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Controllers
{
    public class LignesPanierController : Controller
    {
        private readonly IRepository<LignePanier> _repository;

        public LignesPanierController(IRepository<LignePanier> repository)
        {
            _repository = repository;
        }

        // GET: LignesPanier
        public IActionResult Index()
        {
            var lignesPanier = _repository.GetAll();
            return View(lignesPanier);
        }

        // GET: LignesPanier/Details/5
        public IActionResult Details(int id)
        {
            var lignePanier = _repository.Get(id);
            if (lignePanier == null)
            {
                return NotFound();
            }
            return View(lignePanier);
        }

        // GET: LignesPanier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LignesPanier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,PanierId,ArticleId,Quantite")] LignePanier lignePanier)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(lignePanier);
                return RedirectToAction(nameof(Index));
            }
            return View(lignePanier);
        }

        // GET: LignesPanier/Edit/5
        public IActionResult Edit(int id)
        {
            var lignePanier = _repository.Get(id);
            if (lignePanier == null)
            {
                return NotFound();
            }
            return View(lignePanier);
        }

        // POST: LignesPanier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,PanierId,ArticleId,Quantite")] LignePanier lignePanier)
        {
            if (id != lignePanier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(lignePanier);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repository.Get(lignePanier.Id) == null)
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
            return View(lignePanier);
        }

        // GET: LignesPanier/Delete/5
        public IActionResult Delete(int id)
        {
            var lignePanier = _repository.Get(id);
            if (lignePanier == null)
            {
                return NotFound();
            }

            return View(lignePanier);
        }

        // POST: LignesPanier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
