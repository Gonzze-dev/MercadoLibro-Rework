using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.country
{
    public class CountryRepository(
        TransactionDB transactionDB    
    )
    {
        readonly MercadoLibroContext _context = transactionDB.Context;

        public async Task<IEnumerable<Country>> GetAll() =>
            await _context.Country.ToListAsync();

        public async Task<Country?> Get(string name) =>
            await _context.Country.FirstOrDefaultAsync(c =>
                c.Name == name
            );

        public async Task AddAsync(Country country) =>
            await _context.Country.AddAsync(country);

        public void Update(Country country) =>
            _context.Country.Update(country);

        public void Remove(Country country) =>
            _context.Country.Remove(country);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
        
    }
}
