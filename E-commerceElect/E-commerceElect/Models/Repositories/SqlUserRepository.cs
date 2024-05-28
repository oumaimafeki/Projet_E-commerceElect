using Microsoft.EntityFrameworkCore;
namespace E_commerceElect.Models.Repositories
{
    public class SqlUserRepository : IRepository<ApplicationUser>
    {
        private readonly AppDbContext context;
        public SqlUserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public ApplicationUser Add(ApplicationUser t)
        {
            context.ApplicationUsers.Add(t);
            context.SaveChanges();
            return t;

        }

        public ApplicationUser Delete(int Id)
        {
            ApplicationUser P = context.ApplicationUsers.Find(Id);
            if (P != null)
            {
                context.ApplicationUsers.Remove(P);
                context.SaveChanges();
            }
            return P;
        }

        public ApplicationUser Get(int Id)
        {
            return context.ApplicationUsers.Find(Id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return context.ApplicationUsers;
        }

        public ApplicationUser Update(ApplicationUser t)
        {
            var ApplicationUser = context.ApplicationUsers.Attach(t);
            ApplicationUser.State = EntityState.Modified;
            context.SaveChanges();
            return t;
        }
    }
}
