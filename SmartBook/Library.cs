using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartBook
{
    public class Library
    {
        public List<Book> Books;
        public Library()
        {
            Books = [];
        }

        public bool AddBook(Book book)
        {
            if (Books.Any(b => b.ISBN == book.ISBN))
            {
                UIHelpers.DisplayWarning($"En bok med samma ISBN: '{book.ISBN}' finns redan!");
                return false;  // Returnera false för att indikera misslyckande
            }

            Books.Add(book);
            return true;  // Returnera true för att indikera lyckat tillägg
        }
        public bool RemoveBook(string identifier, string filePath)
        {
            var book = Books.FirstOrDefault(b =>
            b.ISBN.Equals(identifier, StringComparison.OrdinalIgnoreCase) ||
            b.Title.Equals(identifier, StringComparison.OrdinalIgnoreCase));
            if (book == null) return false;

            if (book.IsBorrowed)
            {
                UIHelpers.DisplayWarning("Kan inte ta bort en utlånad bok.");
                return false;
            }

            bool removedBook = Books.Remove(book);
            if (removedBook)
            {
                SaveToFile(filePath);
            }
            return removedBook;
        }

        public List<Book> GetAllBooksSorted()
        {
            return [.. Books.OrderBy(b => b.Title).ThenBy(b => b.Author)];
        }

        public List<Book> Search(string query)
        {
            return [.. Books.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.ISBN.Contains(query, StringComparison.OrdinalIgnoreCase))];
        }
        // I Library.cs
        public int SaveToFile(string filePath)
        {
            try
            {
                Console.WriteLine("\nDebug: Böcker som sparas:");
                foreach (var book in Books)
                {
                    Console.WriteLine($"{book.Title} - Lånad: {book.IsBorrowed}");
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Books, options);
                File.WriteAllText(filePath, json);

                return Books.Count;
            }
            catch (Exception ex)
            {
                UIHelpers.DisplayError($"Fel vid sparning: {ex.Message}");
                return -1;
            }
        }

        public void LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var loadedBooks = JsonSerializer.Deserialize<List<Book>>(json) ?? [];

                    // Lägg till de laddade böckerna till den befintliga listan
                    foreach (var book in loadedBooks)
                    {
                        if (!Books.Any(b => b.ISBN == book.ISBN)) // Undvik dubbletter
                        {
                            Books.Add(book);
                        }
                    }

                    UIHelpers.DisplaySuccess($"📚 Laddade {loadedBooks.Count} bok/böcker från biblioteket (json)\n");
                    UIHelpers.DisplaySuccess($"Totalt antal böcker i biblioteket: {Books.Count}");
                }
                else
                {
                    UIHelpers.DisplayWarning("Ingen sparad fil hittades - fortsätter med nuvarande bibliotek");
                }
            }
            catch (Exception ex)
            {
                UIHelpers.DisplayError($"Kunde inte ladda från fil: {ex.Message}");
            }
            finally
            {
                Thread.Sleep(3000);
            }
        }
    }
}
