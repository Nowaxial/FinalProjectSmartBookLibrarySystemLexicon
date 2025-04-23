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
                Console.Clear();
                Console.WriteLine("SmartBook - Bibliotekssystem\n");

                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║   SmartBook - Bibliotekssystem   ║");
                Console.WriteLine("╠══════════════════════════════════╣");
                Console.WriteLine("║ 1. Lägg till bok                 ║");
                Console.WriteLine("║ 2. Ta bort bok                   ║");
                Console.WriteLine("║ 3. Visa alla böcker              ║");
                Console.WriteLine("║ 4. Sök bok                       ║");
                Console.WriteLine("║ 5. Ändra lånestatus              ║");
                Console.WriteLine("║ 6. Spara bibliotek               ║");
                Console.WriteLine("║ 0. Avsluta                       ║");
                Console.WriteLine("╚══════════════════════════════════╝");

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
                            DisplayWarning("");
                            PauseExecution();
                            //AddBook();
                            break;

                        case "2":
                            DisplayWarning("");
                            PauseExecution();
                            //RemoveBook();
                            break;

                        case "3":
                            DisplayWarning("");
                            PauseExecution();
                            //ListAllBooks();
                            break;

                        case "4":
                            DisplayWarning("");
                            PauseExecution();
                            //SearchAllBooks();
                            break;

                        case "5":
                            DisplayWarning("");
                            PauseExecution();
                            //ToggleBorrowStatus();
                            break;

                        case "6":
                            DisplayWarning("");
                            PauseExecution();
                            break;

                        default:
                            DisplayError($"Felaktigt val: '{input}' | Välj mellan 1-4 eller 0 för att avsluta");
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
    }

}
