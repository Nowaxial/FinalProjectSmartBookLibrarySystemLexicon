namespace SmartBook
{
    public class UIHelpers
    {
        public static Library library = new();
        private const string filePath = "library.json";

        #region Hjälpmetoder

        public static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n⛔ {message}");
            Console.ResetColor();
        }

        public static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {message}");
            Console.ResetColor();
        }

        public static void DisplayWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n⚠  {message}");
            Console.ResetColor();
        }

        private static void ReturnToMainMenu()
        {
            Console.WriteLine("Åter till huvudmenyn....");
            Thread.Sleep(1250);
        }
        private static void ReturnToMenu()
        {
            Console.WriteLine("Åter till menyn....");
            Thread.Sleep(1250);
        }

        public static void PauseExecution(string message = "Tryck <Enter> för att fortsätta...")
        {
            Console.WriteLine(message);

            // Vänta på att användaren trycker Enter
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                // Vänta tills Enter trycks
            }
        }

        public static void SaveToLibrary()
        {
            try
            {
                // Spara biblioteket till fil
                int savedCount = library.SaveToFile(filePath);

                // Om sparningen lyckades, visa meddelande
                if (savedCount >= 0)
                {
                    DisplaySuccess($"📚 Sparade {savedCount} böcker till biblioteket (json)!");
                    DisplaySuccess(
                        $"Totalt antal böcker i biblioteket (json): {library.Books.Count}"
                    );

                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                // Om något går fel, visa felmeddelande
                DisplayError($"Fel: {ex.Message}");
            }
        }

        private static string? GetValidatedInput(string prompt, string errorMessage, bool allowEmptyToCancel = false)
        {
            // Kör tills vi får godkänd input eller användaren avbryter
            while (true)
            {
                // Visar prompten
                string input = GetUserInput(prompt).Trim();

                // Kolla om användaren vill avbryta med enter (eller tom sträng)
                if (allowEmptyToCancel && string.IsNullOrEmpty(input))
                {
                    ReturnToMainMenu();  // Gå tillbaka till menyn
                    return null;          // Tala om att det blev avbrott
                }

                // Om input är OK (inte tom eller bara mellanslag)
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();  // Skicka tillbaka rensad input
                }

                // Input var inte godkänd - visa fel och börja om
                DisplayError(errorMessage);
            }
        }

        private static string? GetValidatedIsbn(bool allowEmptyToCancel = false)
        {
            while (true)
            {
                string input = GetUserInput("ISBN: ").Trim();

                // Kolla om användaren vill avbryta med enter (eller tom sträng)
                if (allowEmptyToCancel && string.IsNullOrEmpty(input))
                {
                    ReturnToMainMenu();
                    return null;
                }

                // Rensa input för analys
                string cleaned = input.Replace("-", "").Replace(" ", "");

                // Ge SPECIFIKA felmeddelanden baserat på problemet

                // Kontrollera längd
                if (cleaned.Length != 13)
                {
                    DisplayError($"Måste vara 13 siffror (du angav {cleaned.Length}).");
                    continue;
                }

                // Kontrollera om det är tomt eller bara bindestreck
                if (!cleaned.All(char.IsDigit))
                {
                    DisplayError("Får endast innehålla siffror och bindestreck.");
                    continue;
                }

                // Använd den befintliga valideringsmetod för slutgiltig godkännande (inne i Book-klassen)
                if (Book.IsValidIsbn(input))
                {
                    return cleaned;
                }
                // Fallback-felmeddelande (bör aldrig triggas om ovan validering är korrekt)
                DisplayError("Ogiltigt ISBN.");
            }
        }

        public static bool AskToContinue(string prompt, string successMessage, string errorMessage = "Ogiltigt val. Ange 'j' för ja eller 'n' för nej.")
        {
            // Kör tills vi får godkänd input
            while (true)
            {
                // Visar prompten
                string response = GetUserInput(prompt).ToLower();

                // Kolla om användaren vill avbryta med ja eller nej
                if (response == "j" || response == "ja")
                {
                    // Om användaren svarar ja, visa framgångsmeddelande
                    if (!string.IsNullOrEmpty(successMessage))
                    {
                        DisplaySuccess(successMessage);
                    }
                    return true;
                }
                // Om användaren svarar nej, går tillbaka till huvudmenyn
                else if (response == "n" || response == "nej")
                {
                    //Return false låter användaren gå tillbaka till huvudmenyn
                    return false;
                }
                else
                {
                    DisplayError(errorMessage);
                }
            }
        }
        private static string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty; //Får tillbaka en tom sträng om användaren inte skriver något
        }

        private static void AddDemoBooks()
        {
            // Skapa en lista med demo böcker
            var demoBooks = new List<Book>
            {
               new Book("Jag lever!", "Darth Sidious", "1234567890", "Psykologi") { IsBorrowed = true },
               new Book("Lever jag?", "Han Solo", "0987654321", "Sci-fi"),
               new Book("Hur ska jag leva?", "Darth Vader", "1122334455", "Filosofi") { IsBorrowed = true },
               new Book("Lever du?", "Leia Organa", "2233445566", "Drama"),
               new Book("Lever vi?", "Obi-Wan Kenobi", "3344556677", "Action") { IsBorrowed = true },
               new Book("Lever de?", "Yoda", "4455667788", "Fantasy"),
               new Book("Lever ni?", "Padmé Amidala", "5566778899", "Romantik") { IsBorrowed = true },
               new Book("Lever hen?", "Chewbacca", "6677889900", "Äventyr"),
            };

            // Räknar antalet böcker som finns innan vi lägger till nya
            int originalCount = library.Books.Count;

            // Lägg endast till böcker som inte redan finns
            foreach (var book in demoBooks.Where(b => !library.Books.Any(lb => lb.ISBN.Equals(b.ISBN, StringComparison.OrdinalIgnoreCase))))
            {
                library.AddBook(book);
            }

            // Räknar ut antalet böcker som lades till
            int addedCount = library.Books.Count - originalCount;


            // Om vi har lagt till böcker, visa meddelande
            if (addedCount > 0)
            {
                DisplaySuccess($"{addedCount}st demo böcker har lagts till i biblioteket!");
            }
            else
            {
                // Om inga böcker lades till, visa meddelande
                DisplayWarning("Inga nya demo böcker lades till - alla böcker fanns redan.");
            }

            PauseExecution();
        }

        private static void HandleReturnBooks()
        {
            while (true) // Loop för att kunna hantera flera böcker
            {
                MenuHelpers.ReturnBookUI();

                {
                    // Visa alla utlånade böcker
                    var borrowedBooks = library.Books
                    .Where(b => b.IsBorrowed)
                    .OrderBy(b => b.Title)
                    .ToList();

                    // Om inga böcker finns, gå tillbaka
                    if (borrowedBooks.Count == 0)
                    {
                        DisplayWarning("Inga utlånade böcker att lämna tillbaka");
                        PauseExecution();
                        return; // Tillbaka till Toggle-menyn
                    }

                    // Visa uppdaterad lista
                    Console.WriteLine("\nUtlånade böcker:\n");
                    Console.WriteLine($"{"Nr".PadRight(5)} {"Titel".PadRight(25)} {"Författare".PadRight(20)} {"Status".PadRight(15)} ");
                    Console.WriteLine(new string('─', 63));

                    for (int i = 0 ; i < borrowedBooks.Count ; i++)
                    {
                        string status = borrowedBooks[i].IsBorrowed ? "🔴 Utlånad" : "🟢 Tillgänglig";
                        Console.WriteLine($"{i + 1,-5} {borrowedBooks[i].Title.PadRight(25)} {borrowedBooks[i].Author.PadRight(20)} {status.PadRight(15)}");
                    }

                    // Bokval - ENTER går tillbaka till Toggle-menyn
                    string choice = GetUserInput("\nAnge boknummer: ");
                    if (string.IsNullOrEmpty(choice))
                    {
                        ReturnToMenu();
                        return; // Tillbaka till Toggle-menyn
                    }

                    // Validera boknumret och kontrollera att det är giltigt
                    if (int.TryParse(choice, out int bookIndex) && bookIndex > 0 && bookIndex <= borrowedBooks.Count)
                    {
                        // Utför återlämning
                        borrowedBooks[bookIndex - 1].ReturnBook();
                        DisplaySuccess($"Boken '{borrowedBooks[bookIndex - 1].Title}' är nu tillgänglig");
                        library.SaveToFile(filePath);

                        // Fråga om att fortsätta lämna tillbaka
                        if (!AskToContinue("Vill du lämna tillbaka en till bok? (j)a / (n)ej: ", ""))
                        {
                            ReturnToMenu();
                            return; // Tillbaka till Toggle-menyn
                        }
                    }
                    else
                    {
                        DisplayError("Ogiltigt boknummer");
                        PauseExecution();
                    }
                }
            }
        }

        private static void HandleBorrowBooks()
        {
            while (true) // Loop för att kunna hantera flera böcker
            {
                MenuHelpers.BorrowBookUI();

                // Visa alla tillgängliga böcker
                var availableBooks = library.Books
                    .Where(b => !b.IsBorrowed)
                    .OrderBy(b => b.Title)
                    .ToList();

                // Om inga böcker finns, gå tillbaka
                if (availableBooks.Count == 0)
                {
                    DisplayWarning("Inga tillgängliga böcker att låna");
                    PauseExecution();
                    return; // Tillbaka till Toggle-menyn
                }

                // Visa uppdaterad lista
                Console.WriteLine("\nTillgängliga böcker:\n");
                Console.WriteLine($"{"Nr".PadRight(5)} {"Titel".PadRight(25)} {"Författare".PadRight(20)} {"Status".PadRight(15)}");
                Console.WriteLine(new string('─', 67));

                for (int i = 0 ; i < availableBooks.Count ; i++)
                {
                    string status = availableBooks[i].IsBorrowed ? "🔴 Utlånad" : "🟢 Tillgänglig";
                    Console.WriteLine($"{i + 1,-5} {availableBooks[i].Title.PadRight(25)} {availableBooks[i].Author.PadRight(20)} {status.PadRight(15)}");
                }

                // Bokval - ENTER går tillbaka till Toggle-menyn
                string choice = GetUserInput("\nAnge boknummer: ");
                if (string.IsNullOrEmpty(choice))
                {
                    ReturnToMenu();
                    return; // Tillbaka till Toggle-menyn
                }

                if (int.TryParse(choice, out int bookIndex) && bookIndex > 0 && bookIndex <= availableBooks.Count)
                {
                    // Utför utlåning
                    availableBooks[bookIndex - 1].BorrowBook();
                    DisplaySuccess($"Boken '{availableBooks[bookIndex - 1].Title}' är nu utlånad");

                    // Fråga om att fortsätta låna ut
                    if (!AskToContinue("Vill du låna ut en till bok? (j)a / (n)ej: ", ""))
                    {
                        return; // Tillbaka till Toggle-menyn
                    }
                }
                else
                {
                    DisplayError("Ogiltigt boknummer");
                    PauseExecution();
                }
            }
        }

        private static void DeleteAllBooksFromLibraryInMemoryAndJsonFile()
        {
            // Tar bort alla böcker i biblioteket
            library.Books.Clear();
            DisplaySuccess("Alla böcker har tagits bort från biblioteket (json och i minnet).");
            PauseExecution();
        }
        #endregion Hjälpmetoder


        public static void MainMenu()
        {
            while (true)
            {
                MenuHelpers.MainMenuUI();
                try
                {
                    string input = GetUserInput("\nVal: ");
                    switch (input)
                    {
                        case "0":
                            Environment.Exit(0);
                            break;

                        case "1":
                            AddBook();
                            break;

                        case "2":
                            RemoveBook();
                            break;

                        case "3":
                            ListAllBooks();
                            break;

                        case "4":
                            SearchAllBooks();
                            break;

                        case "5":
                            ToggleBorrowStatus();
                            break;

                        case "6":
                            SaveToLibrary();
                            break;

                        case "7":
                            library.LoadFromFile(filePath);
                            break;

                        case "8":
                            DeleteAllBooksFromLibraryInMemoryAndJsonFile();
                            break;
                        case "9":
                            AddDemoBooks();
                            break;

                        default:
                            DisplayError(
                                $"Felaktigt val: '{input}' | Välj mellan 1-9 eller 0 för att avsluta"
                            );
                            Thread.Sleep(2000);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError($"Fel vid inläsning: {ex.Message}");
                }
            }
        }




        #region Menymetoderna för huvudmenyn
        private static void AddBook()
        {
            while (true)
            {
                try
                {
                    MenuHelpers.AddBookUI();

                    // Hämta och validera input (med null-check)
                    string? title = GetValidatedInput("Titel: ", "Titel får inte vara tom", allowEmptyToCancel: true);
                    if (title == null) { return; }

                    string? author = GetValidatedInput("Författare: ", "Författare får inte vara tom", allowEmptyToCancel: true);
                    if (author == null) { return; }

                    string? category = GetValidatedInput("Kategori: ", "Kategori får inte vara tom", allowEmptyToCancel: true);
                    if (category == null) { return; }

                    // Hämtar ISBN (med den befintliga valideringen)
                    string? isbn = GetValidatedIsbn(allowEmptyToCancel: true); //Valideras automatiskt av IsValidIsbn metoden inne i GetValidatedIsbn metoden
                    if (isbn == null) { return; }



                    // Kontrollera om boken redan finns i biblioteket och om ISBN är unikt
                    if (library.Books.Any(b => !string.IsNullOrEmpty(b.ISBN) && b.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase)))
                    {
                        DisplayWarning($"Bok med ISBN '{isbn}' finns redan.");
                        continue;
                    }

                    // Skapa bok (vi vet att inga värden är null här)
                    var book = new Book(title, author, isbn, category);
                    if (library.AddBook(book))
                    {
                        DisplaySuccess("Boken lades till!");
                    }

                    if (!AskToContinue("Lägg till fler böcker? (j)a / (n)ej: ", ""))
                    {
                        ReturnToMainMenu();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError($"Fel: {ex.Message}");
                    if (!AskToContinue("Försöka igen? (j)a / (n)ej: ", ""))
                    {
                        ReturnToMainMenu();
                        return;
                    }
                }
            }
        }

        private static void ListAllBooks()
        {
            var books = library.GetAllBooksSorted();

            const int titleWidth = 25;
            const int authorWidth = 25;
            const int isbnWidth = 20;
            const int categoryWidth = 15;
            const int statusWidth = 12;

            MenuHelpers.ListAllBooksUI();
            try
            {
                if (books.Count == 0)
                {
                    DisplayWarning("Inga böcker hittades.");
                }
                else
                {
                    Console.WriteLine(
                        $"{"Titel".PadRight(titleWidth)} "
                            + $"{"Författare".PadRight(authorWidth)} "
                            + $"{"ISBN".PadRight(isbnWidth)} "
                            + $"{"Kategori".PadRight(categoryWidth)} "
                            + $"{"Status".PadRight(statusWidth)}"
                    );
                    Console.WriteLine(
                        new string(
                            '─',
                            titleWidth + authorWidth + isbnWidth + categoryWidth + statusWidth + 6
                        )
                    );

                    foreach (var book in books)
                    {
                        Console.WriteLine($"{book}");
                    }
                }

                Console.WriteLine($"\nTotalt: {library.Books.Count} böcker");
            }
            catch (Exception ex)
            {
                DisplayError($"Fel: {ex.Message}");
            }
            finally
            {
                PauseExecution();
            }
        }

        private static void RemoveBook()
        {
            bool continueRemoving = true;
            var books = library.Books;

            while (continueRemoving)
            {
                MenuHelpers.RemoveBookUI();

                if (library.Books.Count == 0)
                {
                    DisplayWarning("Inga böcker finns att ta bort");
                    PauseExecution();
                    return;
                }

                Console.WriteLine("\nAlla böcker:\n");
                Console.WriteLine(
                    $"{"Titel".PadRight(25)} "
                        + $"{"ISBN".PadRight(20)} "
                        + $"{"Status".PadRight(12)}"
                );
                Console.WriteLine(new string('─', 25 + 20 + 12 + 3));

                var sortedBooks = books.OrderByDescending(b => b.ISBN).ToList();

                foreach (var book in sortedBooks)
                {
                    Console.WriteLine($" {book.ToRemoveString()}");
                }

                string identifier = GetUserInput("\nAnge boktitel eller ISBN att ta bort: ");

                // Lägg till kontroll för ENTER (tom sträng)
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    ReturnToMainMenu();
                    return;
                }

                var bookToRemove = books.FirstOrDefault(b =>
                    b.ISBN.Equals(identifier, StringComparison.OrdinalIgnoreCase)
                    || b.Title.Equals(identifier, StringComparison.OrdinalIgnoreCase)
                );

                if (bookToRemove == null)
                {
                    DisplayError("Boken hittades inte, försök igen!.");
                    PauseExecution();
                    continue;
                }
                else if (bookToRemove.IsBorrowed)
                {
                    DisplayWarning("Kan inte ta bort en utlånad bok.");
                }
                else
                {
                    if (library.RemoveBook(identifier))
                    {
                        DisplaySuccess("Boken har tagits bort från biblioteket");
                    }
                    else
                    {
                        DisplayError("Något gick fel vid borttagning.");
                    }
                }

                if (!AskToContinue("\nVill du ta bort fler böcker? (j)a / (n)ej: ", ""))
                {
                    continueRemoving = false;
                    ReturnToMainMenu();
                    return;
                }
            }
        }

        private static void SearchAllBooks()
        {
            bool searchAgain = true;

            while (searchAgain)
            {
                MenuHelpers.SearchAllBooksUI();

                // Sökning efter böcker genom att fråga användaren
                var query = GetUserInput("Sök på författare, titel eller ISBN: ");

                // Avbryt om användaren trycker ENTER utan input
                if (string.IsNullOrWhiteSpace(query))
                {
                    ReturnToMainMenu();
                    return;
                }

                var results = library.Search(query);

                try
                {
                    if (results.Count == 0)
                    {
                        DisplayWarning("Inga böcker hittades.");
                    }
                    else
                    {
                        Console.WriteLine($"\nSökresultat: ({results.Count})\n");
                        Console.WriteLine(
                            $"{"Titel".PadRight(25)} " +
                            $"{"Författare".PadRight(25)} " +
                            $"{"ISBN".PadRight(13)}"
                        );
                        Console.WriteLine(new string('─', 62));

                        foreach (var book in results)
                        {
                            Console.WriteLine($"{book.ToSearchString()}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayError($"Fel: {ex.Message}");
                }

                // Fråga om användaren vill söka igen med din befintliga AskToContinue-metod
                searchAgain = AskToContinue("\nVill du söka igen? (j)a / (n)ej: ", "");
            }

            PauseExecution();
        }

        private static void ToggleBorrowStatus()
        {
            while (true) // Huvudloop för låne/återlämningsmenyn
            {
                try
                {
                    MenuHelpers.ToggleBorrowStatusUI();

                    // Huvudval - ENTER här ska fortfarande gå till huvudmenyn
                    string menuChoice = GetUserInput("\nVal: ");
                    if (string.IsNullOrEmpty(menuChoice))
                    {
                        ReturnToMainMenu();
                        return;
                    }

                    if (menuChoice == "1")
                    {
                        HandleBorrowBooks(); // Återgår till Toggle-menyn vid ENTER
                    }
                    else if (menuChoice == "2")
                    {
                        HandleReturnBooks(); // Återgår till Toggle-menyn vid ENTER
                    }
                    else
                    {
                        DisplayError("Ogiltigt val. Ange 1, 2 eller tryck ENTER för huvudmeny");
                    }

                    // Fråga inte om fortsättning här - återgå direkt till Toggle-menyn
                }
                catch (Exception ex)
                {
                    DisplayError($"Ett fel uppstod: {ex.Message}");
                    ReturnToMainMenu();
                    return; // Avbryter och gå tillbaka till huvudmenyn
                }
            }
        }
        #endregion Menymetoderna för huvudmenyn
    }
}