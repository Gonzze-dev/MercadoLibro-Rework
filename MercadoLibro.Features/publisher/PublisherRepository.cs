using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.publisher
{
    public class PublisherRepository(
        TransactionDB transactionDB
    )
    {
        readonly MercadoLibroContext _context = transactionDB.Context;

        public async Task<IEnumerable<Publisher>> GetAll() =>
             await _context.Publisher.ToListAsync();

        public async Task<Publisher?> Get(string name) =>
             await _context.Publisher.FirstOrDefaultAsync(p =>
                p.Name == name
            );
        
        public async Task AddAsync(Publisher publisher) =>
            await _context.Publisher.AddAsync(publisher);

        public void Update(Publisher publisher) =>
            _context.Publisher.Update(publisher);

        public void Delete(Publisher publisher) =>
            _context.Publisher.Remove(publisher);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
