# 🏆 Game Tournament Manager

En fullstack-applikation för att hantera spelturneringar med matcher. Byggd med .NET Web API (backend) och vanilla HTML/CSS/JavaScript (frontend).

---

## Vad applikationen gör

- Skapa, visa, redigera och ta bort **turneringar**
- Lägg till och ta bort **matcher** kopplade till en turnering
- Sök efter turneringar via sökfältet
- Feedback visas direkt i gränssnittet vid lyckade/misslyckade operationer

---

## Hur man kör projektet

### Förutsättningar
- .NET 10.0 SDK
- SQL Server (LocalDB fungerar)

### Steg

1. Klona repot:
   ```bash
   git clone https://github.com/benji9812/GameTournamentApi.git
   cd GameTournamentApi

---

## Endpoints

### Tournaments – api/Tournaments

*Metod:		*URL:					*Beskrivning:

GET			/api/Tournaments		Hämta alla turneringar (+ sökning)
GET			/api/Tournaments/{id}	Hämta en turnering
POST		/api/Tournaments		Skapa ny turnering
PUT			/api/Tournaments/{id}	Uppdatera turnering
DELETE		/api/Tournaments/{id}	Ta bort turnering

### Games – api/Games

*Metod:		*URL:				*Beskrivning:

GET			/api/Games			Hämta alla matcher
GET			/api/Games/{id}		Hämta en match
POST		/api/Games			Skapa ny match
PUT			/api/Games/{id}		Uppdatera match
DELETE		/api/Games/{id}		Ta bort match

---

## Hur frontend pratar med API:et:

- Frontenden är en enda HTML-fil (frontend/index.html) utan ramverk. All kommunikation sker via fetch():

- GET /api/Tournaments – hämtar och renderar alla turneringar vid sidladdning och efter varje ändring

- POST /api/Tournaments – formulärdata serialiseras till JSON och skickas med Content-Type: application/json

- PUT /api/Tournaments/{id} – öppnar en modal med förifylld data, PUT:as vid spara

- DELETE /api/Tournaments/{id} och DELETE /api/Games/{id} – bekräftelsedialog, sedan fetch-anrop

- DOM manipuleras direkt via innerHTML och createElement för att rendera listan

- CORS är konfigurerat i Program.cs med AllowAnyOrigin för att tillåta anrop från filsystem.

---

## Reflektion

### Vad gick bra:
Strukturen med Controllers → Services → DTOs kändes naturlig och tydlig. Valideringen med [Required], [Range] och den egna [FutureDate]-attributen gav bra felmeddelanden direkt från .NET utan extra logik. Sökningen via ?search= query-parameter var enkel att lägga till utan att bryta existerande endpoints.

### Vad var svårt:
CORS var det första hindret – frontenden får inga svar alls utan rätt konfiguration. Att hantera datum korrekt mellan datetime-local HTML-input och .NET:s DateTime krävde lite extra arbete, särskilt vid redigering (ISO-sträng → lokal tid → tillbaka).

### Förbättringar:
Med mer tid hade jag lagt till fler statusvärden på turneringar (t.ex. Pågående/Avslutad), paginering om listan växer, och en mer genomarbetad redigeringsvy för matcher.