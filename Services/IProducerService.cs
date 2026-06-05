using BARmonica.Web.Models;

namespace BARmonica.Web.Services
{
    public interface IProducerService
    {
        Task<IEnumerable<Producer>> GetAllProducersAsync();
        Task<Producer?> GetProducerByIdAsync(int id);
        Task AddProducerAsync(Producer producer);
        Task UpdateProducerAsync(Producer producer);
        Task DeleteProducerAsync(int id);
    }
}