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

        public void AddBook(Book book)
        {
            if (Books.Any(b => b.ISBN == book.ISBN))
                UIHelpers.DisplayWarning($"En bok med samma ISBN: '{book.ISBN}' finns redan! ");
            Books.Add(book);
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
            return [.. Books.OrderBy(b => b.Title)];
        }

        public List<Book> Search(string query)
        {
            return [.. Books.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase))];
        }
        public void SaveToFile(string filePath)
        {
            var json = JsonSerializer.Serialize(Books);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                Books = JsonSerializer.Deserialize<List<Book>>(json) ?? [];
            }
        }
    }
}
