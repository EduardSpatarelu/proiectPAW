using BARmonica.Web.Models;

namespace BARmonica.Web.Repositories
{
    public interface IProducerRepository
    {
        Task<IEnumerable<Producer>> GetAllAsync();
        Task<Producer?> GetByIdAsync(int id);
        Task AddAsync(Producer producer);
        Task UpdateAsync(Producer producer);
        Task DeleteAsync(int id);
    }
}