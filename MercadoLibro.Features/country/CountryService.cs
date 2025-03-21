using MercadoLibro.Features.General.DTOs;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.country
{
    public class CountryService(
        CountryRepository repository
    )
    {
        public List<ErrorHttp> Errors = [];
        readonly CountryRepository _repository = repository;

        public async Task<IEnumerable<Country>> GetAll() =>
            await _repository.GetAll();

        public async Task<Country?> Get(string name)
        {
            Country? country;

            if (name is null)
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            country = await _repository.Get(name);

            if (country is null)
            {
                Errors.Add(new ErrorHttp("Country not found", 404));
                return null;
            }

            return country;
        }

        public async Task<Country?> Add(string name)
        {
            Country? country;

            if (name is null)
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            country = await _repository.Get(name);

            if (country is not null)
            {
                Errors.Add(new ErrorHttp("Country already exist", 400));
                return null;
            }

            country = new()
            {
                Name = name
            };

            await _repository.AddAsync(country);

            await _repository.SaveChangesAsync();

            return country;
        }

        public async Task<Country?> Update(
            string name,
            string newName
        )
        {
            Country? country;
            Country? newCountry;

            if (name is null)
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            country = await _repository.Get(name);
            newCountry = await _repository.Get(newName);

            if (country is null)
            {
                Errors.Add(new ErrorHttp("Country not found", 404));
                return null;
            }

            if (newCountry is not null)
            {
                Errors.Add(new ErrorHttp("Country already exist", 400));
                return null;
            }

            country.Name = newName;

            _repository.Update(country);

            await _repository.SaveChangesAsync();

            return country;
        }

        public async Task<Country?> Delete(string name)
        {
            Country? country;

            if (name is null)
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            country = await _repository.Get(name);

            if (country is null)
            {
                Errors.Add(new ErrorHttp("Country not exist", 400));
                return null;
            }

            _repository.Remove(country);

            await _repository.SaveChangesAsync();

            return country;
        }

        public bool HasErrors() =>
            Errors.Count > 0;
    }
}
