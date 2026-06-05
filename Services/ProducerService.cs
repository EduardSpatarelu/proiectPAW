using BARmonica.Web.Models;
using BARmonica.Web.Repositories;

namespace BARmonica.Web.Services
{
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _repository;

        public ProducerService(IProducerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Producer>> GetAllProducersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Producer?> GetProducerByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddProducerAsync(Producer producer)
        {
            await _repository.AddAsync(producer);
        }

        public async Task UpdateProducerAsync(Producer producer)
        {
            await _repository.UpdateAsync(producer);
        }

        public async Task DeleteProducerAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}