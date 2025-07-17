# SmartBook - Bibliotekssystem

## 📚 Beskrivning
SmartBook är ett konsolbaserat bibliotekssystem för hantering av böcker. Programmet innehåller funktioner för att lägga till, ta bort, söka och hantera böcker, samt spara och ladda data till/från JSON-filer.

## 🛠️ Installation från GitHub

1. **Clone'a projektet eller ladda ner**
   ```bash
   git clone [repo-url] 
   ```
   - Eller ladda ner ZIP-filen från GitHub och extrahera

2. **Öppna i Visual Studio 2022**
   - Öppna `SmartBook.sln`
   - Bygg (`Ctrl+Shift+B`) och kör (`F5`)

## ✔️ Testade funktioner (xUnit)

### 📖 Bokhantering
- ISBN-validering (13 siffror)
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
- **Demo-data**: Lägg till testdata för enkel testning
- **Radera bibliotek**: Rensar hela biblioteket (JSON Fil och minne)

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
- Sök böcker efter **exakt matchning** av:
  - **Hela titeln** (t.ex. "Sagan om ringen")
  - **Hela författarens namn** (t.ex. "J.R.R. Tolkien")
  - **Hela ISBN-numret** (t.ex. "9780547928227")

⚠️ **OBS**: Sökningen kräver exakt matchning - partiella matchningar (som "Sagan" eller "Tolk") fungerar inte. För att hitta en bok måste du ange hela söktermen korrekt.

Exempel:
```plaintext
✅ Fungerar:   "Sagan om ringen" → hittar boken
❌ Fungerar inte: "Sagan" → hittar inget

✅ Fungerar:   "J.R.R. Tolkien" → hittar författarens böcker
❌ Fungerar inte: "Tolkien" → hittar inget

✅ Fungerar:   "9780547928227" → hittar boken med detta ISBN
❌ Fungerar inte: "054792822" → hittar inget
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
- Rensa hela biblioteket
- Lägg till demodata för testning

## ❓ Hjälp
Om du stöter på problem:
1. Kontrollera att du har .NET 9.0 installerat
2. Försök "Clean Solution" → "Rebuild Solution"
3. Se till att `library.json` finns i `bin\Debug\net9.0\`


## Begränsningar

- Ingen användarhantering (alla användare delar samma bibliotek)
- Ingen historik över utlåningar
- Ingen avancerad sökning (ex. partiell matchning eller flera sökkriterier)

## Framtida förbättringar

- Implementera användarsystem med individuella lånekort
- Lägg till loggning av utlåningshistorik
- Förbättra sökfunktionen med fler filter
- Möjlighet att exportera rapporter (t.ex. utlånade böcker)
# SmartBook