using GameTournamentApi.Dtos;

namespace GameTournamentApi.Services;

public interface IGameService
{
    Task<GameDto> CreateAsync(CreateGameDto createDto);
    // Skapar ett nytt spel baserat på informationen i CreateGameDto och returnerar det skapade spelet som ett GameDto-objekt.

    Task<IEnumerable<GameDto>> GetAllAsync();
    // Hämtar alla spel i databasen som tillhör en specifik turnering baserat på turneringens ID och returnerar dem som en lista av GameDto-objekt.
    Task<GameDto?> GetByIdAsync(int id);
    // Hämtar ett spel baserat på dess ID och returnerar det som ett GameDto-objekt. Om spelet inte finns, returneras null.

    Task UpdateAsync(int id, UpdateGameDto updateDto);
    // Uppdaterar ett spel baserat på dess ID 

    Task DeleteAsync(int id);
    // Tar bort ett spel baserat på dess ID, returnerar inget (void)

}