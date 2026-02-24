using GameTournamentApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace GameTournamentApi.Dtos;

public class CreateGameDto
{
    // Skapar en Game DTO (Data Transfer Object) för att ta emot data

    //Måste ange titel på spelet, minst 3 tecken
    [Required]
    [MinLength(3)]
    public string Title { get; set; } = string.Empty; // Initialiserar med tom sträng för att undvika null-värden

    //Måste ange tidpunkt för spelet
    [Required]
    [FutureDate]
    public DateTime Time { get; set; }

    // Måste ange ID på turneringen som spelet tillhör
    [Required]
    public int? TournamentId { get; set; } // Frågetecknet gör den nullable
}