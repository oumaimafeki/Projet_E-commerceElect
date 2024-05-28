using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using E_commerceElect.Models;
using E_commerceElect.Models.ViewModels;

public class ClientsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ClientsController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _context.Users.ToListAsync();
        return View(clients);
    }

    public async Task<IActionResult> Commandes(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var commandes = await _context.Commandes
            .Where(c => c.UserId == id)
            .Include(c => c.LignesCommande)
            .ThenInclude(lc => lc.Article)
            .ToListAsync();

        ViewBag.UserName = user.UserName; 
        return View(commandes);
    }

    public async Task<IActionResult> Articles(int id)
    {
        var articles = await _context.LignesCommande
            .Where(lc => lc.CommandeId == id)
            .Include(lc => lc.Article)
            .ToListAsync();
        ViewBag.CommandeId = id; 
        return View(articles);
    }
    public async Task<IActionResult> NonReglees()
    {
        var commandesNonReglees = await _context.Commandes
        .Where(c => c.EstReglee == 0) 
        .Include(c => c.LignesCommande)
        .ThenInclude(lc => lc.Article)
        .ToListAsync();

        return View(commandesNonReglees);
    }
    public async Task<IActionResult> Reglee()
    {
        var commandesReglees = await _context.Commandes
            .Where(c => c.EstReglee == 1)
            .Include(c => c.LignesCommande)
            .ThenInclude(lc => lc.Article)
            .ToListAsync();

        return View(commandesReglees);
    }

    public async Task<IActionResult> ClientsLesPlusFideles()
    {
        var userIds = await _context.Commandes
            .GroupBy(c => c.UserId)
            .Select(g => g.Key)
            .Take(10) 
            .ToListAsync();

        var userNames = new List<string>();

        foreach (var userId in userIds)
        {
            var user = await _userManager.FindByIdAsync(userId);
            userNames.Add(user?.UserName);
        }

        var clientsFideles = await _context.Commandes
            .GroupBy(c => c.UserId)
            .Select(g => new ClientFideleViewModel
            {
                UserId = g.Key,
                TotalAchats = g.Sum(c => c.MontantTotal)
            })
            .OrderByDescending(g => g.TotalAchats)
            .Take(10) 
            .ToListAsync();

        for (int i = 0; i < clientsFideles.Count; i++)
        {
            clientsFideles[i].UserName = userNames[i];
        }

        return View(clientsFideles);
    }
}
