using MercadoLibro.Features.General.DTOs;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.genre
{
    public class GenreService(
        GenreRepository repository
    )
    {
        public List<ErrorHttp> Errors = [];

        readonly GenreRepository _repository = repository;

        public async Task<IEnumerable<Genre>> GetAll() =>
            await _repository.GetAll();

        public async Task<Genre?> Get(string name) =>
            await _repository.Get(name);

        public async Task<Genre?> Add(string name)
        {
            Genre? genre;
            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("name is requiered", 400));
                return null;
            }

            genre = await _repository.Add(name);

            if (genre is null)
                Errors.Add(new ErrorHttp($"the genre {name} already exists", 400));

            await _repository.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre?> Update(string oldName, string newName)
        {
            Genre? genre;

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

            genre = await _repository.Get(oldName);

            if (genre is null)
            {
                Errors.Add(new ErrorHttp($"the genre {oldName} not exists", 400));
                return null;
            }

            genre = await _repository.Update(genre, newName);

            if (genre is null)
            {
                Errors.Add(new ErrorHttp($"the genre {newName} already exists", 400));
                return null;
            }

            await _repository.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre?> Delete(string name)
        {
            Genre? genre;

            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("name is requiered", 400));
                return null;
            }

            genre = await _repository.Delete(name);

            if (genre is null)
            {
                Errors.Add(new ErrorHttp($"the genre {name} not exists", 400));
                return null;
            }

            await _repository.SaveChangesAsync();

            return genre;
        }

        public bool HasErrors() => Errors.Count > 0;

    }
}
