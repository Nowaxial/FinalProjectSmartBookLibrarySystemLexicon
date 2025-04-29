using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
    class MenuHelpers
    {
        public static void MainMenuUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║   SmartBook - Bibliotekssystem   ║");
            Console.WriteLine("╠══════════════════════════════════╣");
            Console.WriteLine("║ 1. Lägg till bok                 ║");
            Console.WriteLine("║ 2. Ta bort bok                   ║");
            Console.WriteLine("║ 3. Visa alla böcker              ║");
            Console.WriteLine("║ 4. Sök bok                       ║");
            Console.WriteLine("║ 5. Låna / lämna tillbaka bok     ║");
            Console.WriteLine("║ 6. Spara bibliotek till fil      ║");
            Console.WriteLine("║ 7. Ladda bibliotek från fil      ║");
            Console.WriteLine("║ 0. Avsluta                       ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }

        public static void AddBookUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║   Lägg till bok i biblioteket    ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }

        public static void RemoveBookUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║   Ta bort bok från biblioteket   ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }

        public static void ListAllBooksUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║  Visa alla böcker i biblioteket  ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }

        public static void SearchAllBooksUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║  Sök efter böcker i biblioteket  ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }

        public static void ToggleBorrowStatusUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║    Låna / lämna tillbaka bok     ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }
    }
}
