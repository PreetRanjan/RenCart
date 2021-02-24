using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RenCart.API.Dtos;
using RenCart.API.Interfaces;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.DataAccess
{
    public class BookService : IBookService, ISaveChange
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;
        private readonly IMapper mapper;
        private readonly IConfiguration config;

        public BookService(AppDbContext db, IWebHostEnvironment env, IMapper mapper,IConfiguration config)
        {
            this.db = db;
            this.env = env;
            this.mapper = mapper;
            this.config = config;
        }
        public async Task<bool> DeleteBook(long bookId)
        {
            try
            {
                var bookToDelete = await db.Books.FindAsync(bookId);
                
                if (bookToDelete.CoverImage != config["AppSettings:DefaultImagePath"])
                {
                    var rootpath = env.WebRootPath;
                    var fullpath = Path.Combine(rootpath, bookToDelete.CategoryName);
                    if (File.Exists(fullpath))
                    {
                        File.Delete(fullpath);
                    }
                }
                

                db.Books.Remove(bookToDelete);
                return await SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks()
        {
            try
            {
                var books = await db.Books
                    .Select(s=>mapper.Map<BookDto>(s))
                    .AsNoTracking()
                    .ToListAsync();
                return books;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<BookDto>> GetAllBooksByPage(int page = 1)
        {
            int take = 10;
            int skip = page > 1 ? page * take : 0;
            var books = await db.Books.Skip(skip).Take(take).Select(s => mapper.Map<BookDto>(s)).AsNoTracking().ToListAsync();
            return books;
        }

        public async Task<BookDto> GetBook(long BookId)
        {
            try
            {
                var book = await db.Books.FindAsync(BookId);
                if (book != null)
                {
                    db.Entry(book).State = EntityState.Detached;
                    return mapper.Map<BookDto>(book);
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<BookDto>> GetBooksInWishList(string wishListId)
        {
            var bookIsList = await db.WishListItems
                .Where(c => c.WishListId == wishListId)
                .Select(x => x.BookId).ToListAsync();
            var books = db.Books.Where(x => bookIsList.Contains(x.Id)).Select(s => mapper.Map<BookDto>(s));
            return await books.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BookDto>> GetSearchedBooks(string searchTerm)
        {
            var books = await db.Books
                .Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()))
                .Select(s => mapper.Map<BookDto>(s))
                .ToListAsync();
            return books;
        }

        public async Task<IEnumerable<BookDto>> GetSimilarBooks(long bookId)
        {
            var book = await GetBook(bookId);
            var similarBooks = await db.Books
                .Where(x => x.CategoryId == book.CategoryId && x.Id != book.Id)
                .Select(s => mapper.Map<BookDto>(s))
                .Take(5)
                .ToListAsync();
            return similarBooks;
        }

        public async Task<bool> SaveBook(BookCreateDto bookDto)
        {
            try
            {
                if (bookDto.BookId == 0)
                {
                    //Add new book
                    var book = mapper.Map<Book>(bookDto);
                    book.CategoryName = (await db.Categories.FindAsync(book.CategoryId)).Name;
                    await UploadPicture(bookDto, book);
                    await db.Books.AddAsync(book);
                    return await SaveChangesAsync();
                }
                else
                {
                    //Update the book
                    var bookToUpdate = await db.Books.FindAsync(bookDto.BookId);
                    bookToUpdate.CategoryName = db.Categories.SingleOrDefault(x => x.Id == bookDto.CategoryId).Name;
                    bookToUpdate.Price = bookDto.Price;
                    bookToUpdate.Title = bookDto.Title;
                    bookToUpdate.Author = bookDto.Author;
                    await UploadPicture(bookDto, bookToUpdate);
                    bookToUpdate.CategoryId = bookDto.CategoryId;

                    db.Entry(bookToUpdate).State = EntityState.Modified;
                    return await SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task UploadPicture(BookCreateDto bookDto, Book bookToUpdate)
        {
            if (bookDto.CoverImage != null)
            {
                var rootpath = env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(bookDto.CoverImage.FileName); ;
                var projectFolder = Path.Combine(@"images", fileName);
                var fullPath = Path.Combine(rootpath, projectFolder);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await bookDto.CoverImage.CopyToAsync(stream);
                    using (var ms = new MemoryStream())
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(ms);
                        bookToUpdate.CoverImageContent = ms.ToArray();
                    }
                }

                bookToUpdate.CoverImage = projectFolder;
            }
            else
            {
                bookToUpdate.CoverImage = config["AppSettings:DefaultImagePath"];
                bookToUpdate.CoverImageContent = null;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await db.SaveChangesAsync() > 1) ? true : false;
        }
    }
}
