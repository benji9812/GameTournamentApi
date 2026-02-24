namespace GameTournamentApi.Dtos
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int MaxPlayers { get; set; }
        public List<GameDto> Games { get; set; } = new List<GameDto>(); // Initialiserar med en tom lista för att undvika null-värden
    }
}