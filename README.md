# MinimalChat

## Cos'è
Una web app di chat a stanze: ti registri, crei una stanza o entri in una esistente e chatti in tempo reale; i messaggi restano salvati e chi entra vede gli ultimi 15.
È il progetto finale (capstone) del bootcamp Epicode, sviluppato in poco più di una settimana.

## Tecnologie usate
- ASP.NET Core 6 MVC + Razor Pages, ASP.NET Core Identity per login e registrazione
- SignalR per i messaggi in tempo reale (client JS 8.0.0 da cdnjs)
- Entity Framework Core 6 con SQL Server, Bootstrap 5.1 e jQuery 3.5.1

## Come avviarlo in locale
Serve un **SQL Server** acceso (anche LocalDB va bene) e l'SDK .NET con il runtime 6.
1. In `MinimalChat/appsettings.json` cambia `MinimalChatDbContextConnection`: ora punta a `DESKTOP-M64O55A`, cioè il mio PC.
2. Crea il database con le migrazioni: `dotnet tool install -g dotnet-ef` e poi, da `MinimalChat/`, `dotnet ef database update`
3. `dotnet run --launch-profile MinimalChat` e apri https://localhost:7028

## Struttura
- `MinimalChat/Controllers` – Home (lista e creazione stanze) e Room (pagina della stanza)
- `MinimalChat/ChatHub.cs` – l'hub SignalR: entrata, uscita e invio dei messaggi, con salvataggio su DB
- `MinimalChat/Areas/Identity` – utente, DbContext e pagine di login/registrazione personalizzate
- `MinimalChat/Models/Domain` e `MinimalChat/Migrations` – le entità `Rooms` e `ChatTexts` e le migrazioni EF
- `MinimalChat/Views` e `MinimalChat/wwwroot` – le viste Razor, il CSS e `js/chat.js` col client SignalR

## Cosa farei diverso oggi
- lo username lo prenderei dal server (`Context.User`) e non dal JS, adesso uno può scrivere a nome di un altro
- i messaggi non li metterei più con `innerHTML`, così è aperto a qualsiasi HTML/script
- lo storico lo manderei solo a chi entra e non a tutta la stanza, ora ogni volta che entra qualcuno gli altri se lo rivedono
- toglierei le cose scritte fisse tipo `https://localhost:7028` nel pulsante EXIT e il nome del mio PC nella connection string
- la passerei a .NET 8, il 6 non è più supportato
