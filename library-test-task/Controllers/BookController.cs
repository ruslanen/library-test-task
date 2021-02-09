using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using library_test_task.Data;
using library_test_task.Data.Models;
using library_test_task.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace library_test_task.Controllers
{
    public class BookController : Controller, IEntityController<BookViewModel>
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookStorage> _bookStorageRepository;
        
        public BookController(IRepository<Book> bookRepository, IRepository<BookStorage> bookStorageRepository)
        {
            _bookRepository = bookRepository;
            _bookStorageRepository = bookStorageRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var book = bookViewModel.ToBook();
            using var transaction = new CommittableTransaction(new TransactionOptions
                {IsolationLevel = IsolationLevel.ReadCommitted});
            try
            {
                var id = await _bookRepository.SaveAsync(book);
                await _bookStorageRepository.SaveAsync(new BookStorage
                {
                    BookId = id,
                    Total = bookViewModel.Total,
                    Free = bookViewModel.Total,
                });
                
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // log error
                throw ex;
            }
            
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromBody] long id)
        {
            var book = await _bookRepository.GetAsync(id);
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] long id)
        {
            using var transaction = new CommittableTransaction(new TransactionOptions
                {IsolationLevel = IsolationLevel.ReadCommitted});
            try
            {
                await _bookRepository.DeleteAsync(id);
                await _bookStorageRepository.GetAll().Where(x => x.BookId == id).DeleteAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var bookStorage = _bookStorageRepository.GetAll();
            var books = await _bookRepository.GetAll()
                .Join(
                    bookStorage.DefaultIfEmpty(),
                    book => book.Id,
                    storage => storage.BookId,
                    (book, storage) => new
                    {
                        book.Id,
                        book.Isbn,
                        book.Title,
                        book.Author,
                        book.PublishYear,
                        storage.Total,
                        storage.Free,
                    }).ToListAsync();
            return Ok(books);
        }
    }
}