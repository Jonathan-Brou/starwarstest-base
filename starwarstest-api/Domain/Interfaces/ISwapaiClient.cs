using Domain.Models;

namespace Domain.Interfaces
{

    public interface ISwapapiClient
    {
        Task<IEnumerable<Character>> GetCharactersFromOriginalTrilogyAsync();

    }
}