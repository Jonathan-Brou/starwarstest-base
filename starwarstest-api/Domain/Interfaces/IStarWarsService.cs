

using Domain.Models;

namespace Domain.Interfaces{

public interface IStarWarsService
{
    Task<List<Character>> GetAllAndSortCharactersAsync();
}
}