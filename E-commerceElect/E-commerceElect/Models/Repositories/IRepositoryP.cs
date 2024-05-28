namespace E_commerceElect.Models.Repositories
{
    public interface IRepositoryP<T>
    {
        T Get(int Id);
        IQueryable<T> GetAll();
        T Add(T t);
        T Update(T t);
        T Delete(int Id);

        void SaveChanges();

    }
}
