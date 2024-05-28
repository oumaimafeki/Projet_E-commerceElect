using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Models.Repositories
{
    public class SqlPanierRepository : IRepositoryP<Panier>
    {
        private readonly AppDbContext context;
        public SqlPanierRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Panier Add(Panier t)
        {
            context.Paniers.Add(t);
            context.SaveChanges();
            return t;
        }

        public Panier Delete(int Id)
        {
            Panier P = context.Paniers.Find(Id);
            if (P != null)
            {
                context.Paniers.Remove(P);
                context.SaveChanges();
            }
            return P;
        }

        public Panier Get(int Id)
        {
            return context.Paniers.Find(Id);
        }

        public IQueryable<Panier> GetAll()
        {
            return context.Paniers;
        }

        public Panier Update(Panier t)
        {
            var Panier = context.Paniers.Attach(t);
            Panier.State = EntityState.Modified;
            context.SaveChanges();
            return t;
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}
