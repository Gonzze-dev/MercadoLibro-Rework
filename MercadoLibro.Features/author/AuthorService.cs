using MercadoLibro.Features.General.DTOs;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.author
{
    public class AuthorService(
        AuthorRepository repository
    )
    {
        public List<ErrorHttp> Errors = [];

        readonly AuthorRepository _repository = repository;

        public async Task<IEnumerable<Author>> GetAll() =>
            await _repository.GetAll();

        public async Task<Author?> Get(string name) =>
            await _repository.Get(name);

        public async Task<Author?> Add(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("name is requiered", 400));
                return null;
            }
            Author? author = await _repository.Add(name);

            if (author is null)
                Errors.Add(new ErrorHttp($"the author {name} already exists", 400));

            await _repository.SaveChangesAsync();

            return author;
        }

        public async Task<Author?> Update(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName))
            {
                Errors.Add(new ErrorHttp("oldName is requiered", 400));
                return null;
            }

            if (string.IsNullOrEmpty(newName))
            {
                Errors.Add(new ErrorHttp("newName is requiered", 400));
                return null;
            }

            Author? author = await _repository.Get(oldName);

            if (author is null) {
                Errors.Add(new ErrorHttp($"the author {oldName} not exists", 400));
                return null;
            }

            author = await _repository.Update(author, newName);

            if (author is null)
            {
                Errors.Add(new ErrorHttp($"the author {newName} already exists", 400));
                return null;
            }

            await _repository.SaveChangesAsync();

            return author;
        }

        public async Task<Author?> Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("name is requiered", 400));
                return null;
            }

            Author? author = await _repository.Delete(name);

            if (author is null)
            {
                Errors.Add(new ErrorHttp($"the author {name} not exists", 400));
                return null;
            }

            await _repository.SaveChangesAsync();

            return author;
        }

        public bool HasErrors() => Errors.Count > 0;

    }
}
