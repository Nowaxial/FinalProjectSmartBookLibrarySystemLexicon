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

        public static void PauseExecution(string message = "Tryck <Enter> för att fortsätta...")
        {
            Console.WriteLine(message);
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                // Vänta tills Enter trycks
            }
        }

        public static void SaveToLibrary()
        {
            try
            {
                int savedCount = library.SaveToFile(filePath);
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
                DisplayError($"Fel: {ex.Message}");
            }
        }

        private static string GetValidatedInput(string prompt, string errorMessage)
        {
            string input;
            do
            {
                input = GetUserInput(prompt);
                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayError(errorMessage);
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        private static string GetValidatedIsbn()
        {
            string isbn;
            bool isValid;
            do
            {
                isbn = GetUserInput("ISBN: ");
                isValid = Book.IsValidIsbn(isbn);
                if (!isValid)
                {
                    DisplayError("Ogiltigt ISBN. Måste vara minst 10 siffror eller bindestreck.");
                }
            } while (!isValid);
            return isbn;
        }

        public static bool AskToContinue(string prompt,string successMessage,string errorMessage = "Ogiltigt val. Ange 'j' för ja eller 'n' för nej.")
        {
            while (true)
            {
                string response = GetUserInput(prompt).ToLower();

                if (response == "j" || response == "ja")
                {
                    if (!string.IsNullOrEmpty(successMessage))
                    {
                        DisplaySuccess(successMessage);
                    }
                    return true;
                }
                else if (response == "n" || response == "nej")
                {
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
            return Console.ReadLine()?.Trim() ?? string.Empty;
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
                            library.Books.Clear();
                            break;

                        default:
                            DisplayError(
                                $"Felaktigt val: '{input}' | Välj mellan 1-8 eller 0 för att avsluta"
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
            bool continueAdding = true;

            while (continueAdding)
            {
                MenuHelpers.AddBookUI();

                // Hämta input
                string title = GetValidatedInput("Titel: ", "Titel får inte vara tom");
                string author = GetValidatedInput("Författare: ", "Författare får inte vara tom");
                string category = GetValidatedInput("Kategori: ", "Kategori får inte vara tom");

                // Hämta ISBN och kontrollera att det inte redan finns
                string isbn;
                bool isbnExists;
                do
                {
                    isbn = GetValidatedIsbn();
                    isbnExists = library.Books.Any(b =>
                        b.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase)
                    );

                    if (isbnExists)
                    {
                        UIHelpers.DisplayWarning(
                            $"En bok med ISBN '{isbn}' finns redan. Ange ett nytt ISBN."
                        );
                    }
                } while (isbnExists);

                try
                {
                    var book = new Book(title, author, isbn, category);
                    if (library.AddBook(book))
                    {
                        DisplaySuccess("Boken har lagts till till i biblioteket!");
                    }

                    while (true)
                    {
                        string response = GetUserInput(
                                "Vill du lägga till fler böcker? (j)a/(n)ej: "
                            )
                            .ToLower();

                        if (response == "j" || response == "ja")
                        {
                            break;
                        }
                        else if (response == "n" || response == "nej")
                        {
                            continueAdding = false;
                            ReturnToMainMenu();
                            return;
                        }
                        else
                        {
                            DisplayError("Ogiltigt val. Ange 'j' för ja eller 'n' för nej.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayError($"Fel: {ex.Message}");
                }
                finally
                {
                    if (continueAdding)
                    {
                        AddBook();
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

                var bookToRemove = books.FirstOrDefault(b =>
                    b.ISBN.Equals(identifier, StringComparison.OrdinalIgnoreCase)
                    || b.Title.Equals(identifier, StringComparison.OrdinalIgnoreCase)
                );

                if (bookToRemove == null)
                {
                    DisplayError("Boken hittades inte.");
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
                while (true)
                {
                    string response = GetUserInput("\nVill du ta bort fler böcker? (j)a/(n)ej: ")
                        .ToLower();

                    if (response == "j" || response == "ja")
                    {
                        break;
                    }
                    else if (response == "n" || response == "nej")
                    {
                        continueRemoving = false;
                        ReturnToMainMenu();
                        return;
                    }
                    else
                    {
                        DisplayError("Ogiltigt val. Ange 'j' för ja eller 'n' för nej.");
                    }
                }
            }
        }

        private static void SearchAllBooks()
        {
            MenuHelpers.SearchAllBooksUI();

            var query = GetUserInput("Sök på författare, titel eller ISBN: ");
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
                        $"{"Titel".PadRight(25)} "
                            + $"{"Författare".PadRight(25)} "
                            + $"{"ISBN".PadRight(25)} "
                    );
                    Console.WriteLine(new string('─', 25 + 25 + 15));

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
            finally
            {
                PauseExecution();
            }
        }

        private static void ToggleBorrowStatus()
        {
            while (true)
            {
                MenuHelpers.ToggleBorrowStatusUI();

                string menuChoice = GetUserInput("\nVal: ");

                if (menuChoice == "0")
                {
                    ReturnToMainMenu();
                    return;
                }

                if (menuChoice == "1")
                {
                    // Låna ut bok flöde
                    MenuHelpers.BorrowBookUI();
                    var availableBooks = library.Books.Where(b => !b.IsBorrowed).ToList();

                    if (availableBooks.Count == 0)
                    {
                        DisplayWarning("Inga tillgängliga böcker att låna ut");
                        PauseExecution();
                        continue;
                    }

                    // Visa tillgängliga böcker
                    Console.WriteLine("\nTillgängliga böcker att låna:\n");
                    Console.WriteLine(
                        $"{"Nr".PadRight(5)} "
                            + $"{"Titel".PadRight(25)} "
                            + $"{"Författare".PadRight(20)} "
                            + $"{"ISBN".PadRight(15)}"
                    );
                    Console.WriteLine(new string('─', 65));

                    for (int i = 0 ; i < availableBooks.Count ; i++)
                    {
                        Console.WriteLine(
                            $"{i + 1,-5} "
                                + $"{availableBooks[i].Title.PadRight(25)} "
                                + $"{availableBooks[i].Author.PadRight(20)} "
                                + $"{availableBooks[i].ISBN.PadRight(15)}"
                        );
                    }

                    string choice = GetUserInput("\nAnge nummer på bok att låna (0 för att avbryta): ");
                    // Om användaren väljer 0, avbryt
                    if (choice == "0")
                        continue;
                    // Kontrollera om valet är ett giltigt nummer
                    if (int.TryParse(choice, out int bookIndex) && bookIndex > 0 && bookIndex <= availableBooks.Count)
                    {
                        availableBooks[bookIndex - 1].BorrowBook();
                        DisplaySuccess($"Boken '{availableBooks[bookIndex - 1].Title}' är nu utlånad");
                        library.SaveToFile(filePath);
                    }
                    else
                    {
                        DisplayError("Ogiltigt boknummer");
                    }
                    PauseExecution();
                }
                else if (menuChoice == "2")
                {
                    // Lämna tillbaka bok flöde
                    MenuHelpers.ReturnBookUI();
                    var borrowedBooks = library.Books.Where(b => b.IsBorrowed).ToList();

                    if (borrowedBooks.Count == 0)
                    {
                        DisplayWarning("Inga utlånade böcker att lämna tillbaka");
                        PauseExecution();
                        continue;
                    }

                    // Visa utlånade böcker
                    Console.WriteLine("\nUlånade böcker att lämna tillbaka:\n");
                    Console.WriteLine(
                        $"{"Nr".PadRight(5)} "
                            + $"{"Titel".PadRight(25)} "
                            + $"{"Författare".PadRight(20)} "
                            + $"{"ISBN".PadRight(15)}"
                    );
                    Console.WriteLine(new string('─', 65));

                    for (int i = 0 ; i < borrowedBooks.Count ; i++)
                    {
                        Console.WriteLine(
                            $"{i + 1,-5} "
                                + $"{borrowedBooks[i].Title.PadRight(25)} "
                                + $"{borrowedBooks[i].Author.PadRight(20)} "
                                + $"{borrowedBooks[i].ISBN.PadRight(15)}"
                        );
                    }

                    string choice = GetUserInput(
                        "\nAnge nummer på bok att lämna tillbaka (0 för att avbryta): "
                    );
                    if (choice == "0")
                        continue;

                    if (int.TryParse(choice, out int bookIndex)&& bookIndex > 0&& bookIndex <= borrowedBooks.Count)
                    {
                        borrowedBooks[bookIndex - 1].ReturnBook();
                        DisplaySuccess(
                            $"Boken '{borrowedBooks[bookIndex - 1].Title}' är nu tillgänglig"
                        );
                        library.SaveToFile(filePath);
                    }
                    else
                    {
                        DisplayError("Ogiltigt boknummer");
                    }
                    PauseExecution();
                }
                else
                {
                    DisplayError("Ogiltigt val. Välj 1, 2 eller 0");
                    PauseExecution();
                }
            }
        }
        #endregion Menymetoderna för huvudmenyn
    }
}