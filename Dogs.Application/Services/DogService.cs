using Dogs.Core.Models;
using Dogs.Core.Parameters;
using Dogs.Core.Repositories;

namespace Dogs.Application.Services;
public class DogService : IDogService
{
    private readonly IDogRepository _dogsRepository;

    public DogService(IDogRepository dogsRepository)
    {
        _dogsRepository = dogsRepository;
    }

    public async Task<IEnumerable<Dog>> GetDogsAsync(QueryParameters queryParameters)
    {
        return await _dogsRepository.GetDogsAsync(queryParameters);
    }

    public async Task<Dog?> GetDogByNameAsync(string name)
    {
        return await _dogsRepository.GetDogByNameAsync(name);
    }

    public async Task<Dog> CreateDogAsync(Dog dog)
    {
        return await _dogsRepository.CreateDogAsync(dog);
    }
}
