using GameTournamentApi.Dtos;
using GameTournamentApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameTournamentApi.Controllers
{
    // För att få automatisk model validation och bättre felhantering.
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        // Skapar en privat readonly fält för att hålla referensen till ITournamentService, som kommer att injiceras via konstruktorn.
        private readonly ITournamentService _service;

        // Injicerar servicen (DI)
        public TournamentsController(ITournamentService service)
        {
            // Sparar den injicerade servicen i ett privat fält så att vi kan använda den i våra action-metoder.
            _service = service;
        }

        // Hämtar en turnering baserat på dess ID. Servicen returnerar en TournamentDto eller null.
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetById(int id)
        {
            // Servicen returnerar nu en DTO (eller null)
            var tournamentDto = await _service.GetByIdAsync(id);

            // Om ingen turnering hittas, returnera 404 Not Found.
            if (tournamentDto == null)
            {
                return NotFound();
            }

            // Om en turnering hittas, returnera den som en DTO.
            return Ok(tournamentDto);
        }

        // Skapar en ny turnering. Tar emot en CreateTournamentDto för att skapa turneringen.
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> Create(CreateTournamentDto createDto)
        {
            // Skapar en ny turnering baserat på den mottagna DTO:n och returnerar den skapade turneringen som en TournamentDto.
            var createdTournament = await _service.CreateAsync(createDto);

            // Returnerar 201 Created med den skapade turneringen. Använder CreatedAtAction.
            return CreatedAtAction(nameof(GetById), new { id = createdTournament.Id }, createdTournament);
        }

        // GET: api/tournaments?search=Mario
        [HttpGet]
        // [FromQuery] betyder att värdet hämtas från URL:en efter frågetecknet
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAll([FromQuery] string? search)
        {
            // Skicka sökordet vidare till servicen
            var tournaments = await _service.GetAllAsync(search);
            return Ok(tournaments);
        }

        // PUT: api/Tournaments/5
        // Uppdaterar en befintlig turnering
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTournamentDto updateDto)
        {
            // 1. Kolla först om turneringen finns (annars får vi fel eller inget svar alls)
            var existingTournament = await _service.GetByIdAsync(id);

            if (existingTournament == null)
            {
                return NotFound(); // Returnerar 404 om id inte finns
            }

            // 2. Anropa servicen för att uppdatera
            await _service.UpdateAsync(id, updateDto);

            // 3. Returnera 204 No Content (Standard vid uppdatering)
            return NoContent();
        }

        // DELETE: api/Tournaments/5
        // Tar bort en turnering
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // 1. Kolla om den finns
            var existingTournament = await _service.GetByIdAsync(id);

            if (existingTournament == null)
            {
                return NotFound();
            }

            // 2. Ta bort
            await _service.DeleteAsync(id);

            // 3. Returnera 204 No Content
            return NoContent();
        }
    }
}
