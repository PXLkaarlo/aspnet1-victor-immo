# Examinerande ASP.NET 1

## CoreFitness

Detta pojektet är en stor övning på delvis att skapa en webapp med MVC, _Model View Controller_, samt förstå funktionaliteten bakom Identity och IdentityRoles.

## Hur man startar projektet.

För att starta programmet så behöver man först öppna CoreFitness.slnx i Visual Studio.
Sedan behöver man Sätta Presentation.WebApp som start projekt.

Detta projekt använder inte en In Memory databas.
Så man behöver koppla programmet till en SQL server.
Kopiera en ConnectionString från en SQL server och klistra in det i **"SqlConnection": "Din-ConnectionString"**.
Den finns i **appsettings.json**.

Sedan i Package Manager Console, skriv **Update-Database**.

Nu borde det fungera att starta projektet.
