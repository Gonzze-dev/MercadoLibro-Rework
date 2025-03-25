using MercadoLibro.Features.General.interfaces;
using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.author
{
    public class AuthorRepository(
        TransactionDB transactionDB    
    ) : IEntityBaseRepository<Author, string>
    {
        readonly MercadoLibroContext _context = transactionDB.Context;
        
        public async Task<IEnumerable<Author>> GetAll() =>
            await _context.Author.ToListAsync();

        public async Task<Author?> Get(string name)
        {
            Author? author = await _context.Author.FirstOrDefaultAsync(author =>
                                author.Name == name
                             );
            
            return author;
        }
        public async Task<Author?> Add(string name)
        {
            Author? author = await Get(name);

            if (author is not null) return null;

            author = new() 
            { 
                Name = name
            };

            await _context.Author.AddAsync(author);

            return author;
        }

        public async Task<Author?> Update(Author uAuthor, string name)
        {
            Author? author = await Get(name);

            if (author is not null) return null;

            uAuthor.Name = name;

            _context.Author.Update(uAuthor);

            return uAuthor;
        }

        public async Task<Author?> Delete(string name)
        {
            Author? author = await Get(name);

            if (author is null) return null;

            _context.Author.Remove(author);

            return author;
        }

        public async Task SaveChangesAsync() => 
            await _context.SaveChangesAsync();
    }
}
