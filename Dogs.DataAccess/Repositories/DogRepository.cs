using Dogs.Core.Models;
using Dogs.Core.Parameters;
using Dogs.Core.Repositories;
using Dogs.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dogs.DataAccess.Repositories;
public class DogRepository : IDogRepository
{
    private readonly DogsDbContext _dbContext;

    public DogRepository(DogsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Dog>> GetDogsAsync(QueryParameters queryParameters)
    {
        IQueryable<DogEntity> query = _dbContext.Dogs.AsNoTracking();

        if (!string.IsNullOrEmpty(queryParameters.Attribute))
        {
            var attribute = queryParameters.Attribute.ToLower();
            var isDesc = queryParameters.Order?.ToLower() == "desc";

            query = attribute switch
            {
                "name" => isDesc ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                "color" => isDesc ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color),
                "tail_length" => isDesc ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
                "weight" => isDesc ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
                _ => query
            };
        }

        if (queryParameters.PageNumber.HasValue && queryParameters.PageSize.HasValue)
        {
            int skip = (queryParameters.PageNumber.Value - 1) * queryParameters.PageSize.Value;
            query = query.Skip(skip).Take(queryParameters.PageSize.Value);
        }

        var dogEntities = await query.ToListAsync();

        var dogs = dogEntities.Select(d => Dog.Create(d.Name, d.Color, d.TailLength, d.Weight));

        return dogs;
    }

    public async Task<Dog?> GetDogByNameAsync(string name)
    {
        var dogEntity = await _dbContext.Dogs.AsNoTracking().FirstOrDefaultAsync(d => d.Name == name);

        if (dogEntity == null)
        {
            return null;
        }

        return Dog.Create(dogEntity.Name, dogEntity.Color, dogEntity.TailLength, dogEntity.Weight);
    }

    public async Task<Dog> CreateDogAsync(Dog dog)
    {
        var existingDog = await _dbContext.Dogs.AsNoTracking().FirstOrDefaultAsync(d => d.Name == dog.Name);

        if (existingDog != null)
        {
            throw new InvalidOperationException("Dog with the same name already exists.");
        }

        var dogEntity = new DogEntity
        {
            Name = dog.Name,
            Color = dog.Color,
            TailLength = dog.TailLength,
            Weight = dog.Weight
        };

        await _dbContext.Dogs.AddAsync(dogEntity);
        await _dbContext.SaveChangesAsync();

        return Dog.Create(dogEntity.Name, dogEntity.Color, dogEntity.TailLength, dogEntity.Weight);
    }
}
