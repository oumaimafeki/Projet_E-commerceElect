using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace E_commerceElect.Models.Repositories
{
    public class SqlCommandeRepository : IRepositoryC<Commande>
    {
        private readonly AppDbContext context;

        public SqlCommandeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Commande Add(Commande t)
        {
            context.Commandes.Add(t);
            context.SaveChanges();
            return t;
        }

        public Commande Delete(int Id)
        {
            Commande P = context.Commandes.Find(Id);
            if (P != null)
            {
                context.Commandes.Remove(P);
                context.SaveChanges();
            }
            return P;
        }

        public Commande Get(int Id)
        {
            return context.Commandes.Find(Id);
        }

        public IEnumerable<Commande> GetAll()
        {
            return context.Commandes;
        }

        public Commande Update(Commande t)
        {
            var Commande = context.Commandes.Attach(t);
            Commande.State = EntityState.Modified;
            context.SaveChanges();
            return t;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
