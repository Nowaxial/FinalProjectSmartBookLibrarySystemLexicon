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
            var book = new Book("Unique Book", "Unique Author", "1234567890125", "Fiction");

            // Act
            bool result = library.AddBook(book);

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
    }
}
