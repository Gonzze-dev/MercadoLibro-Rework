namespace MercadoLibro.Features.General.interfaces
{
    public interface IEntityBaseRepository<TEntity, T> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Get(T id);
        Task<TEntity?> Add(T element);
        Task<TEntity?> Update(TEntity uElement, T element);
        Task<TEntity?> Delete(T element);
        Task SaveChangesAsync();
    }
}
