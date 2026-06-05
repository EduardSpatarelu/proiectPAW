using BARmonica.Web.Data;
using BARmonica.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BARmonica.Web.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly ApplicationDbContext _context;

        public ProducerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producer>> GetAllAsync()
        {
            return await _context.Producers.ToListAsync();
        }

        public async Task<Producer?> GetByIdAsync(int id)
        {
            return await _context.Producers.FindAsync(id);
        }

        public async Task AddAsync(Producer producer)
        {
            await _context.Producers.AddAsync(producer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producer producer)
        {
            _context.Producers.Update(producer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producer = await _context.Producers.FindAsync(id);
            if (producer != null)
            {
                _context.Producers.Remove(producer);
                await _context.SaveChangesAsync();
            }
        }
    }
}