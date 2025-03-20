using MercadoLibro.Features.General.DTOs;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.language
{
    public class LanguageService(
        LangaugeRepository languageRepository
    )
    {
        public List<ErrorHttp> Errors = [];

        readonly LangaugeRepository _repository = languageRepository;

        public async Task<IEnumerable<Language>> GetAll()
        {
            IEnumerable<Language> languages = await _repository.GetAll();

            return languages;
        }

        public async Task<Language?> Add(
            string name
        )
        {
            Language? language;
            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            language = await _repository.Get(name);

            if(language is not null)
            {
                Errors.Add(new ErrorHttp("Language already exist", 400));
                return null;
            }

            language = new()
            { 
                Name = name
            };

            await _repository.AddAsync(language);

            await _repository.SaveChangesAsync();

            return language;
        }

        public async Task<Language?> Update(
            string name,
            string newName
        )
        {
            Language? language;

            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            language = await _repository.Get(name);

            if (language is null)
            {
                Errors.Add(new ErrorHttp("Language not exist", 400));
                return null;
            }

            language.Name = newName;

            _repository.Update(language);

            await _repository.SaveChangesAsync();

            return language;
        }

        public async Task<Language?> Delete(
            string name
        )
        {
            Language? language;

            if (string.IsNullOrEmpty(name))
            {
                Errors.Add(new ErrorHttp("Name is required", 400));
                return null;
            }

            language = await _repository.Get(name);

            if (language is null)
            {
                Errors.Add(new ErrorHttp("Language not exist", 400));
                return null;
            }

            await _repository.Delete(language.Id);

            await _repository.SaveChangesAsync();

            return language;
        }
    }
}
