using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Models.Repositories
{
    public class SqlLigneCommandeRepository : IRepository<LigneCommande>
    {
        private readonly AppDbContext context;
        private List<Commande> llcomm;

        public SqlLigneCommandeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public LigneCommande Add(LigneCommande t)
        {
            context.LignesCommande.Add(t);
            context.SaveChanges();
            return t;
        }

        public LigneCommande Delete(int Id)
        {
            LigneCommande P = context.LignesCommande.Find(Id);
            if (P != null)
            {
                context.LignesCommande.Remove(P);
                context.SaveChanges();
            }
            return P;
        }

        public LigneCommande Get(int Id)
        {
            return context.LignesCommande.Find(Id);
        }

        public IEnumerable<LigneCommande> GetAll()
        {
            return context.LignesCommande;
        }

        public LigneCommande Update(LigneCommande t)
        {
            var LigneCommande = context.LignesCommande.Attach(t);
            LigneCommande.State = EntityState.Modified;
            context.SaveChanges();
            return t;
        }

       

       
    }
}
