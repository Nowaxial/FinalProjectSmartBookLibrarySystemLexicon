using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
    public class UIHelpers
    {
        public static Library library = new();

        const string filePath = "library.json";
        public static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {message}");
            Console.ResetColor();
        }

        public static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n⛔ {message}");
            Console.ResetColor();
        }
        public static void DisplayWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n⚠  {message}");
            Console.ResetColor();
        }

        public static void PauseExecution(string message = "Tryck <Enter> för att fortsätta...")
        {
            Console.WriteLine(message);
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                // Vänta tills Enter trycks
            }
        }

        private static void ReturnToMainMenu()
        {
            Console.WriteLine("Åter till huvudmenyn....");
            Thread.Sleep(1250);
        }

        private static string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? "";
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

                        default:
                            DisplayError($"Felaktigt val: '{input}' | Välj mellan 1-6 eller 0 för att avsluta");
                            PauseExecution();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError($"Fel vid inläsning: {ex.Message}");
                }
            }
        }

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
                    isbnExists = library.Books.Any(b => b.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase));

                    if (isbnExists)
                    {
                        UIHelpers.DisplayWarning($"En bok med ISBN '{isbn}' finns redan. Ange ett nytt ISBN.");
                    }
                } while (isbnExists);

                try
                {
                    var book = new Book(title, author, isbn, category);
                    if (library.AddBook(book))
                    {
                        DisplaySuccess("Boken har lagts till till i biblioteket och sparats till fil!");
                    }

                    while (true)
                    {
                        string response = GetUserInput("Vill du lägga till fler böcker? (j)a/(n)ej: ").ToLower();

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
                        library.SaveToFile(filePath);
                        PauseExecution();
                    }
                }
            }
        }
        private static void RemoveBook()
        {
            MenuHelpers.RemoveBookUI();

            var availableBooks = library.Books.Where(b => !b.IsBorrowed!).ToList();

            if (availableBooks.Count == 0)
            {
                DisplayWarning("Inga böcker finns att ta bort");
                PauseExecution();
                return;
            }

            MenuHelpers.RemoveBookUI();

            Console.WriteLine("\nTillgängliga böcker:\n");

            Console.WriteLine(
                        $"{"Titel".PadRight(25)} " +
                        $"{"ISBN".PadRight(20)} " +
                        $"{"Status".PadRight(12)}");
            Console.WriteLine(new string('─', 25 + 20 + 12 + 3));

            foreach (var book in availableBooks)
            {
                Console.WriteLine($" {book.ToRemoveString()}");
            }

            string identifier = GetUserInput("\nAnge boktitel eller ISBN att ta bort: ");

            if (library.RemoveBook(identifier, filePath))
            {

                DisplaySuccess("Boken har tagits bort från biblioteket");
            }
            else
            {
                DisplayError("Kunde inte hitta boken eller den är utlånad");
            }
            library.SaveToFile(filePath);
            PauseExecution();


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
                        $"{"Titel".PadRight(titleWidth)} " +
                        $"{"Författare".PadRight(authorWidth)} " +
                        $"{"ISBN".PadRight(isbnWidth)} " +
                        $"{"Kategori".PadRight(categoryWidth)} " +
                        $"{"Status".PadRight(statusWidth)}");
                    Console.WriteLine(new string('─', titleWidth + authorWidth + isbnWidth + categoryWidth + statusWidth + 5));

                    foreach (var book in books)
                    {
                        Console.WriteLine($"{book.ToString()}");
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
                        $"{"Titel".PadRight(25)} " +
                        $"{"Författare".PadRight(25)} " +
                        $"{"ISBN".PadRight(25)} ");
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

        public static void SaveToLibrary()
        {
            try
            {
                int savedCount = library.SaveToFile(filePath);
                if (savedCount >= 0)
                {
                    DisplaySuccess($"📚 Sparade {savedCount} böcker till biblioteket!");
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {

                DisplayError($"Fel: {ex.Message}");
            }
        }

        private static void ToggleBorrowStatus()
        {
            MenuHelpers.ToggleBorrowStatusUI();

            var books = library.GetAllBooksSorted();

            if (books.Count == 0)
            {
                DisplayWarning("Inga böcker finns  i biblioteket");
                PauseExecution();
                return;
            }
            Console.WriteLine("\nTillgängliga böcker:\n");
            Console.WriteLine(
                $"{"Nr".PadRight(5)} " +
                $"{"Titel".PadRight(25)} " +
                $"{"ISBN".PadRight(20)} " +
                $"{"Status".PadRight(12)}");

            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine(
            $"{i + 1,-5} " +
            $"{books[i].Title.PadRight(25)} " +
            $"{books[i].ISBN.PadRight(25)} " +
            $"{(books[i].IsBorrowed ? "🔴 Utlånad" : "🟢 Tillgänglig")}");
            }

            string choice = GetUserInput("\nAnge numret på boken som du vill ändra status för (0 för att avbryta: ");

            if (choice == "0")
            {
                ReturnToMainMenu();
                return;
            }
            if (!int.TryParse(choice, out int bookIndex) || bookIndex < 1 || bookIndex > books.Count)
            {
                DisplayError("Felaktigt val. Välj ett nummer från listan");
                PauseExecution();
                return;
            }

            Book selectedBook = books[bookIndex - 1];
            if (selectedBook.IsBorrowed)
            {
                selectedBook.ReturnBook();

            }
            else
            {
                selectedBook.BorrowBook();
            }
            DisplaySuccess($"Status för '{selectedBook.Title}' är nu {(selectedBook.IsBorrowed ? "🔴 Utlånad" : "🟢 Tillgänglig")}");
            library.SaveToFile(filePath);
            PauseExecution();
        }
    }
}