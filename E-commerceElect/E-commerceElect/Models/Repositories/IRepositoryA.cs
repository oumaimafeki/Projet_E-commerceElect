namespace E_commerceElect.Models.Repositories
{
    public interface IRepositoryA<T>
    {
        T Get(int Id);
        IEnumerable<T> GetAll();
        T Add(T t);
        T Update(T t);
        T Delete(int Id);
        List<T> Search(String term);

    }
}
