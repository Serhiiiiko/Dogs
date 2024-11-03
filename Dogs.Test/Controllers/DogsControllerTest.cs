using Dogs.API.Controllers;

namespace Dogs.Test.Controllers;

/// <summary>
/// 3/6 test are passing , need to fix routing in api.controller 
/// </summary>
/// <returns></returns>

public class DogsControllerTests
{
    private readonly Mock<IDogService> _mockDogService;
    private readonly DogsController _controller;

    public DogsControllerTests()
    {
        _mockDogService = new Mock<IDogService>();
        _controller = new DogsController(_mockDogService.Object);
    }
    [Fact]
    public async Task GetDogsAsync_ReturnsOkResult_WithListOfDogs()
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

        _mockDogService.Setup(s => s.GetDogsAsync(queryParameters))
            .ReturnsAsync(dogs);

        var result = await _controller.GetDogsAsync(queryParameters);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnDogs = Assert.IsAssignableFrom<IEnumerable<DogResponse>>(okResult.Value);
        Assert.Equal(2, returnDogs.Count());
    }

    [Fact]
    public async Task GetDogByNameAsync_DogExists_ReturnsOkResult_WithDog()
    {
        var dogName = "Jessy";
        var dog = Dog.Create("Jessy", "black&white", 7, 14);

        _mockDogService.Setup(s => s.GetDogByNameAsync(dogName))
            .ReturnsAsync(dog);

        var result = await _controller.GetDogByNameAsync(dogName);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnDog = Assert.IsType<DogResponse>(okResult.Value);
        Assert.Equal(dog.Name, returnDog.Name);
    }

    [Fact]
    public async Task GetDogByNameAsync_DogDoesNotExist_ReturnsNotFound()
    {
        var dogName = "Unknown";

        _mockDogService.Setup(s => s.GetDogByNameAsync(dogName))
            .ReturnsAsync((Dog?)null);

        var result = await _controller.GetDogByNameAsync(dogName);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Dog with name '{dogName}' not found.", ((dynamic)notFoundResult.Value).Message);
    }

    [Fact]
    public async Task CreateDogAsync_ValidDog_ReturnsCreatedAtAction()
    {
        var dogRequest = new DogRequest("Doggy", "red", 173, 33);
        var createdDog = Dog.Create("Doggy", "red", 173, 33);

        _mockDogService.Setup(s => s.CreateDogAsync(It.IsAny<Dog>()))
            .ReturnsAsync(createdDog);

        var result = await _controller.CreateDogAsync(dogRequest);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnDog = Assert.IsType<DogResponse>(createdAtActionResult.Value);
        Assert.Equal("Doggy", returnDog.Name);
        Assert.Equal("Dogshouseservice.Version1.0.1", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task CreateDogAsync_DuplicateDog_ReturnsConflict()
    {
        var dogRequest = new DogRequest("Jessy", "black&white", 7, 14);

        _mockDogService.Setup(s => s.CreateDogAsync(It.IsAny<Dog>()))
            .ThrowsAsync(new InvalidOperationException("Dog with the same name already exists."));

        var result = await _controller.CreateDogAsync(dogRequest);

        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("Dog with the same name already exists.", ((dynamic)conflictResult.Value).Message);
    }

    [Fact]
    public async Task CreateDogAsync_InvalidModel_ReturnsBadRequest()
    {
        var dogRequest = new DogRequest("", "", -1, 0);
        _controller.ModelState.AddModelError("Name", "Required");
        _controller.ModelState.AddModelError("Color", "Required");
        _controller.ModelState.AddModelError("TailLength", "Must be greater than 0");
        _controller.ModelState.AddModelError("Weight", "Must be greater than 0");

        var result = await _controller.CreateDogAsync(dogRequest);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }
}
