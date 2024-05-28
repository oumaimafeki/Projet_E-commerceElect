using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Models.Repositories
{
    public class SqlCategorieRepository : IRepository<Categorie>
    {
        private readonly AppDbContext context;
        private List<Categorie> lcateg;

        public SqlCategorieRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Categorie Add(Categorie t)
        {
            context.Categories.Add(t);
            context.SaveChanges();
            return t;
        }

        public Categorie Delete(int Id)
        {
            Categorie P = context.Categories.Find(Id);
            if (P != null)
            {
                context.Categories.Remove(P);
                context.SaveChanges();
            }
            return P;
        }

        public Categorie Get(int Id)
        {
            return context.Categories.Find(Id);
        }

        public IEnumerable<Categorie> GetAll()
        {
            return context.Categories;
        }

        public Categorie Update(Categorie t)
        {
            var Categorie = context.Categories.Attach(t);
            Categorie.State = EntityState.Modified;
            context.SaveChanges();
            return t;
        }
        
    }
}
