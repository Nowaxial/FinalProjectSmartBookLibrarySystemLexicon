using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
    public class UIHelpers
    {
        public static Library library = new();
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

        public static void PauseExecution(string message = "\n Tryck <Enter> för att fortsätta...")
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
            return;
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
                            PauseExecution();
                            break;

                        case "6":
                            DisplayWarning("");
                            PauseExecution();
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
            MenuHelpers.AddBookUI();

            string title = GetValidatedInput("Titel: ", "Titel får inte vara tom");
            string author = GetValidatedInput("Författare: ", "Författare får inte vara tom");
            string category = GetValidatedInput("Kategori: ", "Kategori får inte vara tom");
            string isbn = GetValidatedIsbn();

            try
            {
                var book = new Book(title, author, isbn, category);
                if (library.AddBook(book))
                {
                    DisplaySuccess("Boken har lagts till!");
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
            finally
            {
                PauseExecution();
            }
        }
        private static void RemoveBook()
        {
            MenuHelpers.RemoveBookUI();
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
                    Console.WriteLine(new string('─', titleWidth + authorWidth + isbnWidth + categoryWidth+ statusWidth + 3));

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
                else {
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

        private static void ToggleBorrowStatus()
        {
            MenuHelpers.ToggleBorrowStatusUI();
        }
    }
}