# SmartBook - Bibliotekssystem

## 📚 Beskrivning
SmartBook är ett konsolbaserat bibliotekssystem för hantering av böcker. Programmet innehåller funktioner för att lägga till, ta bort, söka och hantera böcker, samt spara och ladda data till/från JSON-filer.

**🚀 Testa live:** [SmartBook](https://smartbook-demo.onrender.com/)
> Free-plan: första laddningen efter idle tar ~30–60 sek. Data nollställs vid omstart.

## 🛠️ Installation från GitHub

1. **Clone'a projektet eller ladda ner**
   ```bash
   git clone https://github.com/Nowaxial/FinalProjectSmartBookLibrarySystemLexicon.git
   ```
   - Eller ladda ner ZIP-filen från GitHub och extrahera

2. **Öppna i Visual Studio 2022**
   - Öppna `SmartBook.sln`
   - Bygg (`Ctrl+Shift+B`) och kör (`F5`)

   Alternativ med terminal:
   ```bash
   dotnet run --project SmartBook/SmartBook.csproj
   dotnet test
   ```

## 🌐 Kör online (Render)

Appen körs med Docker + ttyd som webbterminal:

```bash
docker build -t smartbook .
docker run -p 7681:7681 smartbook
# öppna http://localhost:7681
```

## ✔️ Testade funktioner (xUnit)

### 📖 Bokhantering
- ISBN-validering (13 siffror, både i UI och i `Library.AddBook()`)
- Lägg till böcker (unik ISBN-kontroll)
- Ta bort böcker (endast icke-utlånade)

### 🔄 Utlåningssystem
- Markera böcker som utlånade
- Markera böcker som tillgängliga
- Förhindra borttagning av utlånade böcker

## Menyn i biblioteket

- **Lägg till bok**: Lägg till en ny bok med titel, författare, ISBN och kategori
- **Ta bort bok**: Ta bort en bok via titel eller ISBN (endast om boken inte är utlånad)
- **Visa alla böcker**: Lista alla böcker sorterade efter titel och författare
- **Sök bok**: Sök efter böcker baserat på titel, författare eller ISBN
- **Låna/Återlämna bok**: Markera böcker som utlånade eller tillgängliga
- **Spara/Ladda**: Spara hela biblioteket till JSON-fil eller ladda från befintlig fil
- **Demo-data**: Lägg till testdata med giltiga ISBN-13 för enkel testning
- **Radera bibliotek**: Rensar biblioteket i minnet (filen skrivs över först vid nästa Spara)

## Teknisk implementation

- **Klasser**:
  - `Book`: Representerar en bok med egenskaper och metoder för utlåning
  - `Library`: Hanterar boklistan och filoperationer
  - `UIHelpers`: Hanterar användargränssnitt och validering
  - `MenuHelpers`: Hanterar menyvisning

- **Tekniker**:
  - LINQ för sökning och sortering
  - JSON-serialisering för filsparning
  - Felhantering med try/catch
  - Validering av ISBN (13 siffror)
  - Färgkodad konsolutskrift för bättre användarupplevelse

## Testning

Projektet innehåller xUnit-tester för både `Book`- och `Library`-klasser. Testerna täcker:

- ISBN-validering
- Bokkonstruktion
- Lägga till/ta bort böcker från biblioteket
- Hantering av utlånade böcker

## 🖥️ Systemfunktioner

### 📚 Bokhantering
- Lägg till nya böcker (titel, författare, ISBN, kategori)
- Ta bort böcker (endast icke-utlånade)
- Visa hela boklistan (sorterad efter titel)

### 🔍 Sökfunktioner
- Sök böcker med **partiell matchning** (case-insensitive) på:
  - **Titel** (t.ex. "Sagan om ringen")
  - **Författarens namn** (t.ex. "J.R.R. Tolkien")
  - **ISBN-nummer** (t.ex. "9780547928227")

Exempel:
```plaintext
✅ "Sagan om ringen" → hittar boken
✅ "Sagan" → hittar också boken (del av titel)

✅ "J.R.R. Tolkien" → hittar författarens böcker
✅ "Tolkien" → hittar också (del av namn)

✅ "9780547928227" → hittar boken med detta ISBN
✅ "054792822" → hittar också (del av ISBN)
```

Tips: Använd funktionen "Visa alla böcker" för att se exakta titlar, författare och ISBN-nummer du kan söka efter.

### 🔄 Utlåningssystem
- Låna ut böcker (markerade som tillgängliga)
- Återlämna böcker (markerade som utlånade)
- Visa utlånade böcker
- Visa tillgängliga böcker

### 💾 Datahantering
- Spara hela biblioteket till JSON-fil
- Ladda bibliotek från JSON-fil
- Rensa biblioteket i minnet
- Lägg till demodata för testning

## ❓ Hjälp
Om du stöter på problem:
1. Kontrollera att du har .NET 9.0 installerat (`dotnet --version`)
2. Försök "Clean Solution" → "Rebuild Solution"
3. `library.json` skapas i arbetsmappen där du kör programmet


## Begränsningar

- Ingen användarhantering (alla användare delar samma bibliotek)
- Ingen historik över utlåningar
- Ingen sökning på kategori / status eller flera filter kombinerat
- Ingen persistens på Render Free (filen nollställs vid omstart)

## Framtida förbättringar

- Implementera användarsystem med individuella lånekort
- Lägg till loggning av utlåningshistorik
- Förbättra sökfunktionen med fler filter
- Möjlighet att exportera rapporter (t.ex. utlånade böcker)
