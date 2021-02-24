using RenCart.API.Dtos;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooks();
        Task<IEnumerable<BookDto>> GetAllBooksByPage(int page=1);
        Task<BookDto> GetBook(long BookId);
        Task<bool> SaveBook(BookCreateDto book);
        Task<bool> DeleteBook(long bookId);
        Task<IEnumerable<BookDto>> GetSimilarBooks(long bookId);
        Task<IEnumerable<BookDto>> GetSearchedBooks(string searchTerm);
        Task<IEnumerable<BookDto>> GetBooksInWishList(string wishListId);

    }
}
