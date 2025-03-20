using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.language
{
    public class LangaugeRepository(
        TransactionDB transactionDB
    )
    {
        readonly MercadoLibroContext _context = transactionDB.Context;
        public async Task<IEnumerable<Language>> GetAll() =>
            await _context.Language.ToListAsync();
        
        public async Task<Language?> Get(int id) =>
            await _context.Language.FindAsync(id);

        public async Task<Language?> Get(string name) =>
            await _context.Language.FirstOrDefaultAsync(lan =>
                lan.Name == name
            );

        public async Task AddAsync(Language language) =>
            await _context.Language.AddAsync(language);
        

        public void Update(Language language) =>
            _context.Language.Update(language);


        public async Task<Language?> Delete(int id)
        {
            var language = await _context.Language.FindAsync(id);

            if (language is null) return null;

            _context.Language.Remove(language);

            return language;
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
