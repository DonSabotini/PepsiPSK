namespace PepsiPSK.Data.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> Get(Guid id);
        Task<List<T>> GetAll();
        Task<T> Add(T t);
        Task<T> Update(T t);
        void Delete(T t);
    }
}
