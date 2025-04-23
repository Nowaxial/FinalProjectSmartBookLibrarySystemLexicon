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
            return $"{Title} av {Author} med ISBN: {ISBN} i kategori : {Category} | Status: [{(IsBorrowed ? "Utlånad" : "Tillgänglig")}]";
        }
    }
}