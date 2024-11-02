using System.ComponentModel.DataAnnotations;

namespace Dogs.API.Contracts.Requests;

public record DogRequest(
    [Required]
    [MinLength(1)]
    string Name,

    [Required]
    [MinLength(1)]
    string Color,

    [Required]
    [Range(1, int.MaxValue)]
    int TailLength,

    [Required]
    [Range(1, int.MaxValue)]
    int Weight);
