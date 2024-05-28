using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Models.Repositories
{
    public class SqlLignePanierRepository : IRepository<LignePanier>
    {
        private readonly AppDbContext context;
        private List<Panier> lpanier;

        public SqlLignePanierRepository(AppDbContext context)
        {
            this.context = context;
        }

        public LignePanier Add(LignePanier t)
        {
            context.LignesPanier.Add(t);
            context.SaveChanges();
            return t;
        }

        public LignePanier Delete(int Id)
        {
            LignePanier P = context.LignesPanier.Find(Id);
            if (P != null)
            {
                context.LignesPanier.Remove(P);
                context.SaveChanges();
            }
            return P;
        }

        public LignePanier Get(int Id)
        {
            return context.LignesPanier.Find(Id);
        }

        public IEnumerable<LignePanier> GetAll()
        {
            return context.LignesPanier;
        }

        public LignePanier Update(LignePanier t)
        {
            var LignePanier = context.LignesPanier.Attach(t);
            LignePanier.State = EntityState.Modified;
            context.SaveChanges();
            return t;
        }
       
    }
}
