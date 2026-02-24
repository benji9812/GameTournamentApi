using GameTournamentApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace GameTournamentApi.Dtos;

public class UpdateTournamentDto
{
    // Måste ange titel på turneringen, minst 3 tecken
    [Required]
    [MinLength(3)]
    public string Title { get; set; } = string.Empty;
    
    //Beskrivning
    public string Description { get; set; } = string.Empty;

    // Måste ange startdatum för turneringen
    [Required]
    [FutureDate]
    public DateTime StartDate { get; set; }

    // Måste ange max antal spelare, mellan 2 och 100
    [Range(2, 100)]
    public int MaxPlayers { get; set; }
}