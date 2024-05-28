using Microsoft.AspNetCore.Mvc;
using E_commerceElect.Models;
using E_commerceElect.Models.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerceElect.Models.ViewModels;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Authorization;

namespace E_commerceElect.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly IRepositoryA<Article> _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IRepository<Categorie> _categorieRepository;
        private readonly AppDbContext _context; 


        public ArticlesController(IRepositoryA<Article> repository, IWebHostEnvironment hostingEnvironment, IRepository<Categorie> categorieRepository, AppDbContext context)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _categorieRepository = categorieRepository;
            _context = context;
        }
        [AllowAnonymous]
        // GET: Articles
        public IActionResult Index(int? categorieId, string priceRange)
        {
            var categories = _categorieRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Nom");

            var articles = _repository.GetAll();

            if (categorieId.HasValue)
            {
                articles = articles.Where(a => a.CategorieId == categorieId.Value);
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange)
                {
                    case "0-50":
                        articles = articles.Where(a => a.Prix < 500);
                        break;
                    case "50-100":
                        articles = articles.Where(a => a.Prix >= 500 && a.Prix <= 1500);
                        break;
                    case "100-200":
                        articles = articles.Where(a => a.Prix > 1500);
                        break;
                }
            }

            return View(articles);
        }

        // GET: Articles/Details/5
        public IActionResult Details(int id)
        {
            var article = _repository.Get(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            var categories = _categorieRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Nom");
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
           
                try
                {
                    string uniqueFileName = null;
                    if (model.ImageUrl != null)
                    {
                        uniqueFileName = ProcessUploadedFile(model.ImageUrl);
                    }

                    Article newArticle = new Article
                    {
                        Nom = model.Nom,
                        Description = model.Description,
                        Prix = model.Prix,
                        CategorieId = model.CategorieId,
                        Marque = model.Marque,
                        ImageUrl = uniqueFileName
                    };

                    _repository.Add(newArticle);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while creating the article: {ex.Message}");
                }
            

            var categories = _categorieRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Nom");
            return View(model);
        }


        // GET: Articles/Edit/5
        public IActionResult Edit(int id)
        {
            var article = _repository.Get(id);
            if (article == null)
            {
                return NotFound();
            }

            var categories = _categorieRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Nom");

            EditViewModel articleEditViewModel = new EditViewModel
            {
                Id = article.Id,
                Nom = article.Nom,
                Description = article.Description,
                Prix = article.Prix,
                CategorieId = article.CategorieId,
                Marque = article.Marque,
                ExistingImagePath = article.ImageUrl
            };

            return View(articleEditViewModel);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            try
            {
                var article = _repository.Get(model.Id);
                if (article == null)
                {
                    return NotFound();
                }

                article.Nom = model.Nom;
                article.Description = model.Description;
                article.Prix = model.Prix;
                article.CategorieId = model.CategorieId;
                article.Marque = model.Marque;

                if (model.ImageUrl != null)
                {
                    if (model.ExistingImagePath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingImagePath);
                        System.IO.File.Delete(filePath);
                    }

                    article.ImageUrl = ProcessUploadedFile(model.ImageUrl);
                }

                _repository.Update(article);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_repository.Get(model.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the article: {ex.Message}");
            }

            var categories = _categorieRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Nom");
            return View(model);
        }
        // GET: Articles/Delete/5
        public IActionResult Delete(int id)
        {
            var article = _repository.Get(id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _repository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while deleting the article: {ex.Message}");
                return View();
            }
        }

        [NonAction]
        private string ProcessUploadedFile(IFormFile imageUrl)
        {
            string uniqueFileName = null;
            if (imageUrl != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageUrl.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageUrl.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public ActionResult Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return RedirectToAction("Index");
            }

            var result = _repository.Search(term);
            return View("Index", result);
        }
        public async Task<IActionResult> TopVentes()
        {
            var topVentes = await _context.LignesCommande
       .GroupBy(lc => new { lc.ArticleId, lc.Article.Nom })
       .Select(g => new
       {
           ArticleId = g.Key.ArticleId,
           Nom = g.Key.Nom,
           TotalQuantite = g.Sum(lc => lc.Quantite)
       })
       .OrderByDescending(a => a.TotalQuantite)
       .ToListAsync();

            return View(topVentes);
        }
    }
}
