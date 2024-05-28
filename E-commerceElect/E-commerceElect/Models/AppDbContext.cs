using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Models
{
    public class AppDbContext : IdentityDbContext

	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LignesCommande { get; set; }
        public DbSet<Panier> Paniers { get; set; }
        public DbSet<LignePanier> LignesPanier { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Categorie)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategorieId);

            modelBuilder.Entity<Commande>()
                .Property(c => c.UserId)
                .IsRequired(); 

            modelBuilder.Entity<LigneCommande>()
                .HasOne(lc => lc.Commande)
                .WithMany(c => c.LignesCommande)
                .HasForeignKey(lc => lc.CommandeId);

            modelBuilder.Entity<LigneCommande>()
                .HasOne(lc => lc.Article)
                .WithMany()
                .HasForeignKey(lc => lc.ArticleId);

            modelBuilder.Entity<Panier>()
              .Property(p => p.UserId)
              .IsRequired(); 

            modelBuilder.Entity<LignePanier>()
                .HasOne(lp => lp.Panier)
                .WithMany(p => p.LignePaniers)
                .HasForeignKey(lp => lp.PanierId);

            modelBuilder.Entity<LignePanier>()
                .HasOne(lp => lp.Article)
                .WithMany()
                .HasForeignKey(lp => lp.ArticleId);

            modelBuilder.Entity<ApplicationUser>()
               .HasKey(u => u.UtilisateurId);
        }
    }
}
