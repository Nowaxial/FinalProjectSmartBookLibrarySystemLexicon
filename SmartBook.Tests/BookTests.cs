namespace SmartBook.Tests
{
    public class BookTests
    {
        [Fact]
        public void Does_The_Book_Contructor_Set_Properties_Right() //Testar om konstruktorn sätter rätt egenskaper.
        {
            //Arrange
            string title = "Test Book";
            string author = "Test Author";
            string isbn = "4598789678894";
            string category = "Test Category";

            //Act
            var book = new Book(title, author, isbn, category); // Skapar en bok med egenskaperna ovan.

            //Assert
            Assert.Equal(title, book.Title); // Kollar om titeln är rätt.
            Assert.Equal(author, book.Author); // Kollar om författaren är rätt.
            Assert.Equal(isbn, book.ISBN); // Kollar om ISBN är rätt.
            Assert.Equal(category, book.Category); // Kollar om kategorin är rätt.
            Assert.False(book.IsBorrowed); // Kollar om boken är tillgänglig (IsBorrowed = false).
        }

        [Theory]
        //Testar olika ISBN-nummer som är giltiga och ogiltiga och att ISBN är 13 siffror.
        [InlineData("4598789678894", true)]
        [InlineData("459-878-9678894", true)]
        [InlineData("459878967889", false)]
        [InlineData("45987896788949", false)]
        [InlineData("4598789678894LOL", false)]
        [InlineData("", false)] //Tom sträng
        [InlineData(" ", false)] //Mellanslag
        [InlineData(null, false)] //Null värde
        public void IsValidIsbn_Returning_Correct_Validation_Result(string isbn, bool expected) //Testar om IsValidIsbn metoden returnerar rätt värde.
        {
            //Act
            bool result = Book.IsValidIsbn(isbn); // Kollar om ISBN är giltigt.

            //Assert
            Assert.Equal(expected, result); // Kollar om resultatet är rätt och om det stämmer med förväntat värde.
        }

        [Fact]
        public void BorrowBook_Should_Set_IsBorrowed_To_True() // Testar om BorrowBook metoden sätter IsBorrowed till true (utlånad).
        {
            // Arrange
            var book = new Book("Test Book", "Test Author", "4598789678894", "Test Category"); // Skapar en bok med IsBorrowed = false som är satt som standard.

            // Act
            book.BorrowBook(); //Lånar ut boken (sätter IsBorrowed till true).

            // Assert
            Assert.True(book.IsBorrowed); // Kollar om boken är utlånad och IsBorrowed = true.
        }

        [Fact]
        public void ReturnBook_Should_Set_IsBorrowed_To_False() // Testar om ReturnBook metoden sätter IsBorrowed till false (tillgänglig)
        {
            // Arrange
            var book = new Book("Test Book", "Test Author", "4598789678894", "Test Category"); // Skapar en bok med IsBorrowed = false som är satt som standard
            book.BorrowBook(); // Sätter till true först för utlånad bok

            // Act
            book.ReturnBook(); // Sätter IsBorrowed till false (tillgänglig).

            // Assert
            Assert.False(book.IsBorrowed); // Kollar om boken är tillgänglig (IsBorrowed = false).
        }
    }
}
