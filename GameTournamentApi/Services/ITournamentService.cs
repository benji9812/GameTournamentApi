using GameTournamentApi.Dtos;

namespace GameTournamentApi.Services;

public interface ITournamentService
{
    Task<IEnumerable<TournamentDto>> GetAllAsync(string? searchTitle);
    // Hämtar alla turneringar, returnerar en lista av Dto

    Task<TournamentDto?> GetByIdAsync(int id);
    // Hämtar en turnering via ID, returnerar null om den inte finns

    Task<TournamentDto> CreateAsync(CreateTournamentDto createDto);
    // Skapar en ny turnering, returnerar den skapade turneringen som Dto

    Task UpdateAsync(int id, UpdateTournamentDto updateDto);
    // Uppdaterar en turnering via ID, returnerar inget (void)

    Task DeleteAsync(int id);
    // Tar bort en turnering via ID, returnerar inget (void)

}