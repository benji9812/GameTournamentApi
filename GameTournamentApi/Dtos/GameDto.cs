namespace GameTournamentApi.Dtos;

public class GameDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Time { get; set; }
    public int TournamentId { get; set; }
}