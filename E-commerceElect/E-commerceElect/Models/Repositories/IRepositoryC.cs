namespace E_commerceElect.Models.Repositories
{
    public interface IRepositoryC<T>
    {
        T Add(T entity);
        T Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        T Update(T entity);
        void SaveChanges();
    }

}
