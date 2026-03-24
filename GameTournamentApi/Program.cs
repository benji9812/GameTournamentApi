using GameTournamentApi.Services;
using Microsoft.EntityFrameworkCore;
using GameTournamentApi.Data;

// Skapar en WebApplicationBuilder som används för att konfigurera och bygga applikationen.
var builder = WebApplication.CreateBuilder(args);

// Lägger till Services i DI-containern
builder.Services.AddControllers();

// Behövs för Swagger för att kunna hitta och dokumentera alla controllers och deras endpoints
builder.Services.AddEndpointsApiExplorer();

// Genererar Swagger-dokumentet som beskriver API:et, inklusive alla controllers och deras endpoints
builder.Services.AddSwaggerGen();

// Databaskoppling med Entity Framework Core och SQL Server
builder.Services.AddDbContext<TournamentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Service-injektioner för DI (Dependency Injection)
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IGameService, GameService>();

// CORS-konfiguration för att tillåta frontend-applikationen att göra förfrågningar till API:et.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Bygger applikationen, vilket skapar en WebApplication-instans som kan hantera HTTP-förfrågningar.
var app = builder.Build();

// Aktiverar Swagger UI endast i utvecklingsmiljön, så att det inte är tillgängligt i produktion.
if (app.Environment.IsDevelopment())
{
    // Skapar JSON-filen
    app.UseSwagger();

    // Skapar webbsidan
    app.UseSwaggerUI();
}

// Aktiverar CORS med den policy som definierats tidigare, så att frontend-applikationen kan kommunicera med API:et utan problem.
app.UseCors("AllowFrontend");

// Middleware som hanterar HTTP-förfrågningar. De körs i den ordning de är definierade här.
app.UseHttpsRedirection();

// Mappar inkommande HTTP-förfrågningar till rätt controller och action baserat på URL och HTTP-metod.
app.UseAuthorization();

// Controllers och deras actions kopplas in i pipeline:n, så att de kan hantera inkommande förfrågningar.
app.MapControllers();

// Startar applikationen och börjar lyssna efter inkommande HTTP-förfrågningar.
app.Run();