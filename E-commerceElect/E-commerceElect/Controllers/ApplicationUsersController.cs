using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerceElect.Models;
using System.Threading.Tasks;
using System.Linq;

namespace E_commerceElect.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly AppDbContext context;


        public ApplicationUsersController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            return View(await context.ApplicationUsers.ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.UtilisateurId == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UtilisateurId,Nom,Prenom,Adresse,MotDePasse")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                context.Add(applicationUser);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await context.ApplicationUsers.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UtilisateurId,Nom,Prenom,Adresse,MotDePasse")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.UtilisateurId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(applicationUser);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.UtilisateurId))
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
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.UtilisateurId == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationUser = await context.ApplicationUsers.FindAsync(id);
            context.ApplicationUsers.Remove(applicationUser);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(int id)
        {
            return context.ApplicationUsers.Any(e => e.UtilisateurId == id);
        }
    }
}
