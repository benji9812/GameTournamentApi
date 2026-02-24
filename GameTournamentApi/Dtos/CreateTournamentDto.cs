using GameTournamentApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace GameTournamentApi.Dtos
{
    public class CreateTournamentDto
    {
        // Skapar en Tournament DTO (Data Transfer Object) för att ta emot data

        // Måste ange titel på turneringen, minst 3 tecken
        [Required]
        [MinLength(3)]
        public string Title { get; set; } = string.Empty; // Initialiserar med tom sträng för att undvika null-värden

        // Beskrivning
        public string Description { get; set; } = string.Empty; // Beskrivning är inte obligatorisk, så den kan vara tom

        // Måste ange startdatum för turneringen
        [Required]
        [FutureDate]
        public DateTime StartDate { get; set; }

        // Måste ange max antal spelare, mellan 2 och 100
        [Range(2, 100)]
        public int MaxPlayers { get; set; }
    }
}