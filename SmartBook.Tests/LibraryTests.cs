namespace SmartBook.Tests
{
    public class LibraryTests
    {
        private Library CreateTestLibrary() 
        {
            var library = new Library();
            library.AddBook(new Book("Test Book 1", "Author A", "1234567890123", "Fiction"));
            library.AddBook(new Book("Test Book 2", "Author B", "1234567890124", "Non-Fiction"));
            return library;
        }

        [Fact]
        public void AddBook_Should_Add_Book_When_ISBN_Is_Unique()
        {
            // Arrange
            var library = new Library();
            var book = new Book("Unique Book", "Unique Author", "1234567890125", "Fiction"); // Skapar en ny bok med ett unikt ISBN

            // Act
            bool result = library.AddBook(book); // Försöker lägga till boken i biblioteket

            // Assert

            Assert.True(result); // Kollar om boken lades till
            Assert.Contains(book, library.Books); // Kollar om boken finns i biblioteket
            Assert.Equal(book, library.Books.First()); // Kollar om boken är den första i listan
        }

        [Fact]

        public void AddBook_Should_Not_Add_Book_When_ISBN_Is_Not_Unique() //Kollar om boken inte lades till om det finns en annan bok med samma ISBN
        {
            // Arrange
            var library = CreateTestLibrary(); // Skapar ett bibliotek med två böcker som redan finns skapade av CreateTestLibrary metoden
            var sameIsbnBook = new Book("Duplicate Book", "Duplicate Author", "1234567890123", "Fiction"); //Kollar om boken har samma ISBN som en annan bok i biblioteket

            // Act
            bool result = library.AddBook(sameIsbnBook); // Försöker lägga till boken med samma ISBN

            // Assert
            Assert.False(result); // Kollar om boken inte lades till
            Assert.Equal(2, library.Books.Count); // Kollar om det fortfarande finns 2 böcker i biblioteket
        }

        [Fact]
        public void Remove_Book_Should_Remove_Book_When_It_Exists_And_Not_Borrowed()
        {
            // Arrange
            var library = CreateTestLibrary(); // Skapar ett testbibliotek med 2 böcker
            string isbnToRemove = "1111111111111"; // ISBN för boken vi vill ta bort

            // Lägg till en ny bok som INTE är utlånad och som vi kan ta bort
            var bookToRemove = new Book("Boken att ta bort", "Test Författare", isbnToRemove, "Test") ;

            // Lägg till boken i biblioteket
            library.AddBook(bookToRemove);

            // Antal böcker innan borttagning (förväntat 3: 2 från CreateTestLibrary + 1 vi la till)
            int initialCount = library.Books.Count;

            // Act
            bool result = library.RemoveBook(isbnToRemove); // Försök ta bort boken

            // Assert
            Assert.True(result); // Kontrollera att borttagningen lyckades
            Assert.Equal(initialCount - 1, library.Books.Count); // Kontrollera att antalet böcker minskat med 1
            Assert.DoesNotContain(library.Books, b => b.ISBN == isbnToRemove); // Kontrollera att boken verkligen är borta
        }
    }
}
