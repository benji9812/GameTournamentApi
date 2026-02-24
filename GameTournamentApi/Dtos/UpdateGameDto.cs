using GameTournamentApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace GameTournamentApi.Dtos;

public class UpdateGameDto
{
    [Required]
    [MinLength(3)]
    public string Title { get; set; } = string.Empty; // Initialiserar med tom sträng för att undvika null-värden

    [Required]
    [FutureDate]
    public DateTime Time { get; set; }

    [Required]
    public int TournamentId { get; set; } // Om man vill flytta matchen till en annan turnering
}