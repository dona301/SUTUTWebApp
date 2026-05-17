# SUTUTWebApp

## Preduvjeti

Prije pokretanja aplikacije potrebno je imati instalirano sljedeće:

- [Visual Studio](https://visualstudio.microsoft.com/)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

---

## Instalacija i pokretanje

### 1. Kloniranje repozitorija

```bash
git https://github.com/dona301/SUTUTWebApp.git
```

Otvoriti projekt u Visual Studiju odabirom datoteke `SUTUTWebApp.sln`.

---

### 2. Kreiranje baze podataka

U SSMS-u se spojiti na lokalnu instancu SQL Servera te kreirati novu praznu bazu podataka.

---

### 3. Konfiguracija connection stringa

U datoteci `appsettings.json` promijeniti connection string da odgovara lokalnoj bazi:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SUTUTWebApp;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

### 4. Pokretanje migracija

U terminalu unutar Visual Studia (ili PowerShell-u) pokrenuti sljedeću naredbu kako bi se stvorila struktura baze podataka:

```bash
dotnet ef database update
```

---

### 5. Punjenje baze početnim podacima

U SSMS-u otvoriti novi Query prozor, učitati datoteku `inserting_data.sql` koja se nalazi u korijenu repozitorija te pokrenuti skriptu.

---

### 6. Pokretanje aplikacije

Aplikaciju pokrenuti u Visual Studiju odabirom profila **http** i klikom na gumb za pokretanje (ili tipka `F5`) ili naredbom

```bash
dotnet run --launch-profile http
```

Aplikacija će se otvoriti u pregledniku na adresi `http://localhost:{port}`.

---

## Pokretanje testova

```bash
cd .\SUTUTWebApp.Tests\
dotnet test
```
