using Dogs.API.Contracts.Requests;
using Dogs.API.Contracts.Responses;
using Dogs.Core.Models;
using Dogs.Core.Parameters;
using Dogs.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dogs.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DogsController : ControllerBase
{
    private readonly IDogService _dogService;

    public DogsController(IDogService dogService)
    {
        _dogService = dogService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DogResponse>>> GetDogsAsync([FromQuery] QueryParameters queryParameters)
    {
        var dogs = await _dogService.GetDogsAsync(queryParameters);

        var response = dogs.Select(d => new DogResponse(d.Name, d.Color, d.TailLength, d.Weight));

        return Ok(response);
    }


    /// <summary>
    /// Needs to be fixed. Wrong routing on create method, gives 500 exception, but still creates dogs.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Routes unhandled exception, creates dog</returns>
    [HttpPost]
    public async Task<ActionResult<DogResponse>> CreateDogAsync([FromBody] DogRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var dog = await _dogService.CreateDogAsync(Dog.Create(request.Name, request.Color, request.TailLength, request.Weight));

            var response = new DogResponse(dog.Name, dog.Color, dog.TailLength, dog.Weight);

            return CreatedAtAction(nameof(GetDogByNameAsync), new { name = dog.Name }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<DogResponse>> GetDogByNameAsync(string name)
    {
        var dog = await _dogService.GetDogByNameAsync(name);

        if (dog == null)
        {
            return NotFound();
        }

        var response = new DogResponse(dog.Name, dog.Color, dog.TailLength, dog.Weight);

        return Ok(response);
    }
}
