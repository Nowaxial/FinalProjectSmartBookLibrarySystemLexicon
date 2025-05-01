using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBook.Interface;

namespace SmartBook
{
    public class Book(string title, string author, string isbn, string category) : ISearchBook
    {
        public string Title { get; private set; } = title;
        public string Author { get; private set; } = author;
        public string ISBN { get; private set; } = isbn;
        public string Category { get; private set; } = category;
        public bool IsBorrowed { get; set; } = false;

        public void BorrowBook() => IsBorrowed = true;
        public void ReturnBook() => IsBorrowed = false;

        public override string ToString()
        {
            return string.Format("{0,-25} {1,-25} {2,-20} {3,-15} {4,-12}",
        Title,
        Author,
        ISBN,
        Category,
        IsBorrowed ? "🔴 Utlånad" : "🟢 Tillgänglig");
        }
        public static bool IsValidIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            // Ta bort alla bindestreck och mellanslag
            string cleanedIsbn = isbn.Replace("-", "").Replace(" ", "");

            // ISBN-13 måste ha exakt 13 siffror
            return cleanedIsbn.Length == 13 &&
                   cleanedIsbn.All(char.IsDigit);
        }

        public string ToSearchString()
        {
            return string.Format("{0,-25} {1,-25} {2,-13}",
                Title,
                Author,
                ISBN);
        }

        public string ToRemoveString()
        {
            return string.Format("{0,-25}{1,-20}{2,-12}", Title, ISBN, IsBorrowed ? "🔴 Utlånad" : "🟢 Tillgänglig");
        }
    }
}