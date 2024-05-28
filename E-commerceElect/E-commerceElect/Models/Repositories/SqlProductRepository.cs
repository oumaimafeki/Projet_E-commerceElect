using Microsoft.EntityFrameworkCore;

namespace E_commerceElect.Models.Repositories
{
    public class SqlProductRepository : IRepositoryA<Article>
    {
        private readonly AppDbContext context;
        private List<Article> larticle;

        public SqlProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Article Add(Article t)
        {
            context.Articles.Add(t);
            context.SaveChanges();
            return t;
        }

        public Article Delete(int Id)
        {
            Article P = context.Articles.Find(Id);
            if (P != null)
            {
                context.Articles.Remove(P);
                context.SaveChanges();
            }
            return P;

        }

        public Article Get(int Id)
        {
            return context.Articles.Find(Id);

        }

        public IEnumerable<Article> GetAll()
        {
            return context.Articles;
        }

        public Article Update(Article t)
        {
            var Article =context.Articles.Attach(t);
            Article.State = EntityState.Modified;
            context.SaveChanges();
            return t;

        }
        public List<Article>Search (String term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                return context.Articles.Where(a => a.Marque.Contains(term)).ToList();
            }
            else
            {
                return GetAll().ToList(); // Retourne tous les articles si le terme de recherche est vide
            }
        }
    }
}
