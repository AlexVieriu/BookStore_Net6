﻿using BookStore.UI.Server.Models;

namespace BookStore.UI.Server.Services.Book;
public interface IBookService
{
    Task<Response<BookReadDtoVirtualizeResponse>> GetWithPG(QueryParameters queryParams);
    Task<Response<BookReadDto>> GetBook(int id);
    Task<Response<List<BookReadDto>>> GetBooks();
    Task<Response<int>> CreateBook(BookCreateDto book);
    Task<Response<int>> UpdateBook(int id, BookUpdateDto book);
    Task<Response<int>> DeleteBook(int id);
}
