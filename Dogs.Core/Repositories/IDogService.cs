﻿using Dogs.Core.Models;
using Dogs.Core.Parameters;

namespace Dogs.Core.Repositories;
public interface IDogService
{
    Task<IEnumerable<Dog>> GetDogsAsync(QueryParameters queryParameters);
    Task<Dog?> GetDogByNameAsync(string name);
    Task<Dog> CreateDogAsync(Dog dog);
}