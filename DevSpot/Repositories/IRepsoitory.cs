namespace DevSpot.Repositories
{
    public interface IRepsoitory<T> where T: class // I can use this repository where generic type is a Class (it means that I can ONLY use this on classes)
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
