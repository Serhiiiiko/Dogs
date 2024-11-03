using Dogs.Application.Services;

namespace Dogs.Test.Services;

/// <summary>
/// Passes all tests for the DogService class.
/// </summary>

public class DogServiceTest
{
    private readonly Mock<IDogRepository> _mockDogRepository;
    private readonly IDogService _dogService;

    public DogServiceTest()
    {
        _mockDogRepository = new Mock<IDogRepository>();
        _dogService = new DogService(_mockDogRepository.Object);
    }

    [Fact]
    public async Task GetDogsAsync_ReturnsListOfDogs()
    {
        var queryParameters = new QueryParameters
        {
            Attribute = "weight",
            Order = "desc",
            PageNumber = 1,
            PageSize = 10
        };

        var dogs = new List<Dog>
            {
                Dog.Create("Neo", "red&amber", 22, 32),
                Dog.Create("Jessy", "black&white", 7, 14)
            };

        _mockDogRepository.Setup(r => r.GetDogsAsync(queryParameters))
            .ReturnsAsync(dogs);

        var result = await _dogService.GetDogsAsync(queryParameters);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateDogAsync_ValidDog_ReturnsCreatedDog()
    {
        var dog = Dog.Create("Doggy", "red", 173, 33);

        _mockDogRepository.Setup(r => r.CreateDogAsync(dog))
            .ReturnsAsync(dog);

        var result = await _dogService.CreateDogAsync(dog);

        Assert.NotNull(result);
        Assert.Equal(dog.Name, result.Name);
    }

    [Fact]
    public async Task CreateDogAsync_DuplicateDog_ThrowsInvalidOperationException()
    {
        var dog = Dog.Create("Jessy", "black&white", 7, 14);

        _mockDogRepository.Setup(r => r.CreateDogAsync(dog))
            .ThrowsAsync(new InvalidOperationException("Dog with the same name already exists."));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _dogService.CreateDogAsync(dog));
    }

    [Fact]
    public async Task GetDogByNameAsync_DogExists_ReturnsDog()
    {
        var dogName = "Jessy";
        var dog = Dog.Create("Jessy", "black&white", 7, 14);

        _mockDogRepository.Setup(r => r.GetDogByNameAsync(dogName))
            .ReturnsAsync(dog);

        var result = await _dogService.GetDogByNameAsync(dogName);

        Assert.NotNull(result);
        Assert.Equal(dogName, result.Name);
    }

    [Fact]
    public async Task GetDogByNameAsync_DogDoesNotExist_ReturnsNull()
    {
        var dogName = "Unknown";

        _mockDogRepository.Setup(r => r.GetDogByNameAsync(dogName))
            .ReturnsAsync((Dog?)null);

        var result = await _dogService.GetDogByNameAsync(dogName);

        Assert.Null(result);
    }
}
