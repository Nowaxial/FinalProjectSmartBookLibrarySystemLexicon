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
        public bool RemoveBook(string identifier)
        {
            var book = Books.FirstOrDefault(b => b.ISBN == identifier || b.Title == identifier);
            if (book != null)
            {
                return Books.Remove(book);
            }
            return false;
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
        public void SaveToFile(string filePath)
        {
            try
            {
                // Ladda först befintliga böcker från filen
                List<Book> existingBooks = [];
                if (File.Exists(filePath))
                {
                    var existingJson = File.ReadAllText(filePath);
                    existingBooks = JsonSerializer.Deserialize<List<Book>>(existingJson) ?? [];
                }

                // Slå samman och ta bort dubbletter
                var allBooks = Books.UnionBy(existingBooks, b => b.ISBN).ToList();

                // Spara den kombinerade listan
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(allBooks, options);
                File.WriteAllText(filePath, json);

                Console.WriteLine($"📚 Sparade {allBooks.Count} bok/böcker till biblioteket");
            }
            catch (Exception ex)
            {
                UIHelpers.DisplayError($"Fel: {ex.Message}");
            }
            finally {
                Thread.Sleep(3000);
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

                    UIHelpers.DisplaySuccess($"📚 Laddade {loadedBooks.Count} bok/böcker från biblioteket");
                    Console.WriteLine($"Totalt antal böcker i biblioteket: {Books.Count}");
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
