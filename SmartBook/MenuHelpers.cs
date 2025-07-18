﻿using System;
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
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║     SmartBook - Bibliotekssystem     ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 1. Lägg till bok                     ║");
            Console.WriteLine("║ 2. Ta bort bok                       ║");
            Console.WriteLine("║ 3. Visa alla böcker                  ║");
            Console.WriteLine("║ 4. Sök bok                           ║");
            Console.WriteLine("║ 5. Låna / lämna tillbaka bok         ║");
            Console.WriteLine("║ 6. Spara bibliotek till fil          ║");
            Console.WriteLine("║ 7. Ladda bibliotek från fil          ║");
            Console.WriteLine("║ 8. Radera biblioteket (json + minne) ║");
            Console.WriteLine("║ 9. Lägg till demo böcker             ║");
            Console.WriteLine("║ 0. Avsluta                           ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
        }

        public static void AddBookUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║   Lägg till bok i biblioteket    ║");
            Console.WriteLine("║   Tryck ENTER för att avbryta    ║");
            Console.WriteLine("╚══════════════════════════════════╝\n");
        }

        public static void RemoveBookUI()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║   Ta bort bok från biblioteket   ║");
            Console.WriteLine("║   Tryck ENTER för att avbryta    ║");
            Console.WriteLine("╚══════════════════════════════════╝");
        }

        public static void ListAllBooksUI()
        {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════╗");
            Console.WriteLine("║  Alla böcker i biblioteket  ║");
            Console.WriteLine("╚═════════════════════════════╝\n");
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
            Console.WriteLine("╔═════════════════════════════════════════════╗");
            Console.WriteLine("║          Låna / lämna tillbaka bok          ║");
            Console.WriteLine("╠════════════════════════════════════════════ ╣");
            Console.WriteLine("║ 1. Låna en bok                              ║");
            Console.WriteLine("║ 2. Lämna tillbaka en bok                    ║");
            Console.WriteLine("║ Tryck 'ENTER' för att komma till huvudmenyn ║");
            Console.WriteLine("╚═════════════════════════════════════════════╝");
        }

        public static void BorrowBookUI()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║            Låna bok               ║");
            Console.WriteLine("║ Tryck 'ENTER' för att gå tillbaka ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
        }

        public static void ReturnBookUI()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║        Lämna tillbaka bok         ║");
            Console.WriteLine("║ Tryck 'ENTER' för att gå tillbaka ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
        }
    }
}
