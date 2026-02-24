using Microsoft.AspNetCore.Mvc;
using GameTournamentApi.Services;
using GameTournamentApi.Dtos;

namespace GameTournamentApi.Controllers;

// För att få automatisk model validation och bättre felhantering.
[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    // Skapar en privat readonly fält för att hålla referensen till IGameService, som kommer att injiceras via konstruktorn.
    private readonly IGameService _service;

    // Injicerar servicen (DI)
    public GamesController(IGameService service)
    {
        // Sparar den injicerade servicen i ett privat fält så att vi kan använda den i våra action-metoder.
        _service = service;
    }

    // Skapar en ny match. Tar emot en CreateGameDto.
    [HttpPost]
    public async Task<ActionResult<GameDto>> Create(CreateGameDto createdDto)
    {
        // Anropar CreateAsync på servicen för att skapa en ny match baserat på den data som skickats in i DTO:n.
        var createdGame = await _service.CreateAsync(createdDto);
        
        // Returnerar den skapade matchen som ett GameDto. Använder Ok() för att indikera att det lyckades.
        return Ok(createdGame);
    }

    // Hämtar alla matcher. Returnerar en lista av GameDto.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetAll()
    {
        // Anropar GetAllAsync på servicen för att hämta alla matcher.
        var games = await _service.GetAllAsync();
        
        // Returnerar listan av matcher som en IEnumerable<GameDto>. Använder Ok() för att indikera att det lyckades.
        return Ok(games);
    }

    // Hämtar en match baserat på dess ID. Tar emot ID som en parameter i URL:en.
    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> GetById(int id)
    {
        // Anropar GetByIdAsync på servicen för att hämta en match baserat på dess ID.
        var game = await _service.GetByIdAsync(id);
        
        // Om ingen match hittas, returnera 404 Not Found.
        if (game == null)
        {
            return NotFound();
        }
        
        // Om en match hittas, returnera den som ett GameDto.
        return Ok(game);
    }

    // PUT: api/Games/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateGameDto updateDto)
    {
        // 1. Kolla om matchen finns
        var existingGame = await _service.GetByIdAsync(id);

        if (existingGame == null)
        {
            return NotFound();
        }

        // 2. Uppdatera
        await _service.UpdateAsync(id, updateDto);

        // 3. Returnera 204 No Content
        return NoContent();
    }

    // DELETE: api/Games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingGame = await _service.GetByIdAsync(id);

        if (existingGame == null)
        {
            return NotFound();
        }

        await _service.DeleteAsync(id);

        return NoContent();
    }
}