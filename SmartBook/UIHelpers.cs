using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
    public class UIHelpers
    {
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
            Console.WriteLine($"\n⚠ {message}");
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

        private static bool CheckForExit(string input)
        {
            if (input?.ToLower() != "exit")
            {
                return false;
            }

            ReturnToMainMenu();
            return true;
        }

        public static void MainMenu()
        {
            while (true)
            {
                MenuHelpers.MainMenuUI();

                string input = GetUserInput("\nVal: ");
                if (CheckForExit(input)) continue;

                // TRY-BLOCK - Här börjar exceptionhanteringen
                try
                {
                    switch (input)
                    {
                        case "0":
                            Environment.Exit(0);
                            break;

                        case "1":
                            AddBook();
                            PauseExecution();
                            break;

                        case "2":
                            RemoveBook();
                            PauseExecution();
                            break;

                        case "3":
                            ListAllBooks();
                            PauseExecution();
                            break;

                        case "4":
                            SearchAllBooks();
                            PauseExecution();
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
                catch (Exception ex)  // CATCH-BLOCK - Här hamnar alla exceptions

                {
                    DisplayError($"Fel vid inläsning: {ex.Message}");
                }
            }
        }

        private static void ToggleBorrowStatus()
        {
            MenuHelpers.ToggleBorrowStatusUI();
        }

        private static void SearchAllBooks()
        {
            MenuHelpers.SearchAllBooksUI();
        }

        private static void ListAllBooks()
        {
            MenuHelpers.ListAllBooksUI();
        }

        private static void RemoveBook()
        {
            MenuHelpers.RemoveBookUI();
        }

        private static void AddBook()
        {
            MenuHelpers.AddBookUI();
        }
    }

}
