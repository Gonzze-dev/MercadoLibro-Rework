using MercadoLibro.Features.General.DTOs;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.publisher
{
    public class PublisherService(
        PublisherRepository publisherRepository
    )
    {
        public List<ErrorHttp> Errors = [];
        readonly PublisherRepository _repository = publisherRepository;

        public async Task<IEnumerable<Publisher>> GetAll() =>
            await _repository.GetAll();

        public async Task<Publisher?> Add(
            string name
        )
        {
            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            Publisher? publisher = await _repository.Get(name);

            if (publisher is not null)
            {
                Errors.Add(new ErrorHttp("Publisher already exists", 400));
                return null;
            }
            publisher = new()
            {
                Name = name
            };

            await _repository.AddAsync(publisher);

            await _repository.SaveChangesAsync();

            return publisher;
        }

        public async Task<Publisher?> Update(
            string name,
            string newName
        )
        {
            if(string.IsNullOrEmpty(name))
                Errors.Add(new ErrorHttp("Name is required", 400));

            if (string.IsNullOrEmpty(newName))
                Errors.Add(new ErrorHttp("New name is required", 400));

            if (HasErrors()) return null;

            Publisher? publisher = await _repository.Get(name);

            if (publisher is null)
            {
                Errors.Add(new ErrorHttp("Publisher not exists", 400));
                return null;
            }

            publisher.Name = newName;

            _repository.Update(publisher);

            await _repository.SaveChangesAsync();

            return publisher;
        }

        public async Task<Publisher?> Delete(
            string name
        )
        {
            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            Publisher? publisher = await _repository.Get(name);

            if (publisher is null)
            {
                Errors.Add(new ErrorHttp("Publisher not exists", 400));
                return null;
            }

            _repository.Delete(publisher);

            await _repository.SaveChangesAsync();

            return publisher;
        }

        public bool HasErrors() => Errors.Count > 0;
    }
}
