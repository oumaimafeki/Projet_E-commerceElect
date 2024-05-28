using Microsoft.AspNetCore.Mvc;
using E_commerceElect.Models;
using E_commerceElect.Models.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace E_commerceElect.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IRepository<Categorie> _repository;


        public CategoriesController(IRepository<Categorie> repository)
        {
            _repository = repository;
        }
        [AllowAnonymous]
        // GET: Categories
        public IActionResult Index()
        {
            var categories = _repository.GetAll();
            return View(categories);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int id)
        {
            var categorie = _repository.Get(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nom")] Categorie categorie)
        {
            try
            {
                _repository.Add(categorie);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)

            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
            return View(categorie);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            var categorie = _repository.Get(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nom")] Categorie categorie)
        {
            try
            {
                if (id != categorie.Id)
                {
                    return NotFound();
                }


                _repository.Update(categorie);
                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

        
            return View(categorie);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int id)
        {
            Categorie categorie = _repository.Get(id);
            if (categorie == null)
            {
                return NotFound();
            }

            return View(categorie);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
