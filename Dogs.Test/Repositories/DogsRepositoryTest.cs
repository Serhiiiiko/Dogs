using Dogs.DataAccess.Entities;
using Dogs.DataAccess.Repositories;
using Dogs.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Dogs.Test.Repositories;

/// <summary>
/// Passes all the tests for the DogRepository class.
/// </summary>

public class DogRepositoryTests
{
    private DbContextOptions<DogsDbContext> _dbContextOptions;

    public DogRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DogsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private DogsDbContext GetDbContext()
    {
        return new DogsDbContext(_dbContextOptions);
    }

    [Fact]
    public async Task CreateDogAsync_ValidDog_AddsDogToDatabase()
    {
        using var context = GetDbContext();
        var repository = new DogRepository(context);
        var dog = Dog.Create("Doggy", "red", 173, 33);

        var createdDog = await repository.CreateDogAsync(dog);

        var dogEntity = await context.Dogs.FirstOrDefaultAsync(d => d.Name == "Doggy");
        Assert.NotNull(dogEntity);
        Assert.Equal("red", dogEntity.Color);
        Assert.Equal(173, dogEntity.TailLength);
        Assert.Equal(33, dogEntity.Weight);
        Assert.Equal(createdDog.Name, dogEntity.Name);
    }

    [Fact]
    public async Task CreateDogAsync_DuplicateDog_ThrowsInvalidOperationException()
    {
        using var context = GetDbContext();
        context.Dogs.Add(new DogEntity { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 });
        await context.SaveChangesAsync();

        var repository = new DogRepository(context);
        var dog = Dog.Create("Jessy", "black&white", 7, 14);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.CreateDogAsync(dog));
    }

    [Fact]
    public async Task GetDogsAsync_ReturnsFilteredAndPagedDogs()
    {
        using var context = GetDbContext();
        context.Dogs.AddRange(
            new DogEntity { Name = "Dog1", Color = "brown", TailLength = 10, Weight = 20 },
            new DogEntity { Name = "Dog2", Color = "black", TailLength = 15, Weight = 25 },
            new DogEntity { Name = "Dog3", Color = "white", TailLength = 5, Weight = 10 }
        );
        await context.SaveChangesAsync();

        var repository = new DogRepository(context);
        var queryParameters = new QueryParameters
        {
            Attribute = "weight",
            Order = "desc",
            PageNumber = 1,
            PageSize = 2
        };

        var dogs = await repository.GetDogsAsync(queryParameters);

        Assert.Equal(2, dogs.Count());
        Assert.Equal("Dog2", dogs.First().Name); 
        Assert.Equal("Dog1", dogs.Skip(1).First().Name);
    }

    [Fact]
    public async Task GetDogByNameAsync_DogExists_ReturnsDog()
    {
        using var context = GetDbContext();
        context.Dogs.Add(new DogEntity { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 });
        await context.SaveChangesAsync();

        var repository = new DogRepository(context);

        var dog = await repository.GetDogByNameAsync("Jessy");

        Assert.NotNull(dog);
        Assert.Equal("Jessy", dog.Name);
        Assert.Equal("black&white", dog.Color);
    }

    [Fact]
    public async Task GetDogByNameAsync_DogDoesNotExist_ReturnsNull()
    {
        using var context = GetDbContext();
        var repository = new DogRepository(context);

        var dog = await repository.GetDogByNameAsync("Unknown");

        Assert.Null(dog);
    }
}
