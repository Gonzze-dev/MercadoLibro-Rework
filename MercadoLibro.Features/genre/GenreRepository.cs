using MercadoLibro.Features.General.interfaces;
using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.genre
{
    public class GenreRepository(
        TransactionDB transactionDB    
    ) : IEntityBaseRepository<Genre, string>
    {
        readonly MercadoLibroContext _context = transactionDB.Context;

        public async Task<IEnumerable<Genre>> GetAll() =>
            await _context.Genre.ToListAsync();

        public async Task<Genre?> Get(string name)
        {
            Genre? genre = await _context.Genre.FirstOrDefaultAsync(author =>
                                author.Name == name
                             );
            
            return genre;
        }
        public async Task<Genre?> Add(string name)
        {
            Genre? genre = await Get(name);

            if (genre is not null) return null;

            genre = new() 
            { 
                Name = name
            };

            await _context.Genre.AddAsync(genre);

            return genre;
        }

        public async Task<Genre?> Update(Genre uGenre, string name)
        {
            Genre? genre = await Get(name);

            if (genre is not null) return null;

            uGenre.Name = name;

            _context.Genre.Update(uGenre);

            return uGenre;
        }

        public async Task<Genre?> Delete(string name)
        {
            Genre? genre = await Get(name);

            if (genre is null) return null;

            _context.Genre.Remove(genre);

            return genre;
        }

        public async Task SaveChangesAsync() => 
            await _context.SaveChangesAsync();
    }
}
