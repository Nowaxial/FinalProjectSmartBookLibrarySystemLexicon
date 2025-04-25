using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
    public class Book(string title, string author, string isbn, string category)
    {
        public string Title { get; private set; } = title;
        public string Author { get; private set; } = author;
        public string ISBN { get; private set; } = isbn;
        public string Category { get; private set; } = category;
        public bool IsBorrowed { get; private set; } = false;

        public void BorrowBook() => IsBorrowed = true;
        public void ReturnBook() => IsBorrowed = false;

        public override string ToString()
        {
            return $"📖 Titel: {Title}\n   👤 Författare: {Author}\n   🏷️ Kategori: {Category}\n   🔢 ISBN: {ISBN}\n   {(IsBorrowed ? "🔴 UTLÅNAD" : "🟢 TILLGÄNGLIG")}";
        }
        public static bool IsValidIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn)) return false;
            return isbn.Length >= 10 && isbn.All(c => char.IsDigit(c) || c == '-');
        }
    }
}