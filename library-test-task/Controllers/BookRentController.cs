using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using library_test_task.Data;
using library_test_task.Data.Models;
using library_test_task.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_test_task.Controllers
{
    public class BookRentController : Controller
    {
        private readonly IRepository<BookStorage> _bookStorage;
        private readonly IRepository<BookRent> _bookRent;

        public BookRentController(IRepository<BookStorage> bookStorage, IRepository<BookRent> bookRent)
        {
            _bookStorage = bookStorage;
            _bookRent = bookRent;
        }

        [HttpGet]
        public async Task<IActionResult> List(BookRentFilterViewModel bookRentFilterViewModel)
        {
            if (bookRentFilterViewModel == null)
            {
                var list = await _bookRent.GetAll().ToListAsync();
                return Ok(list);
            }

            List<Expression<Func<BookRent, bool>>> filters = new List<Expression<Func<BookRent, bool>>>();
            if (!string.IsNullOrEmpty(bookRentFilterViewModel.BookFilter))
            {
                Expression<Func<BookRent, bool>> bookFilterExp = x =>
                    x.Book.Title.Contains(bookRentFilterViewModel.BookFilter) ||
                    x.Book.Author.Contains(bookRentFilterViewModel.BookFilter);
                filters.Add(bookFilterExp);
            }

            if (!string.IsNullOrEmpty(bookRentFilterViewModel.NameFilter))
            {
                Expression<Func<BookRent, bool>> nameFilterExp = x =>
                    x.Customer.LastName.Contains(bookRentFilterViewModel.NameFilter)
                    || x.Customer.FirstName.Contains(bookRentFilterViewModel.NameFilter)
                    || x.Customer.Patronymic.Contains(bookRentFilterViewModel.NameFilter);
                filters.Add(nameFilterExp);
            }

            if (bookRentFilterViewModel.RentStatusFilter != RentStatusEnum.All)
            {
                Expression<Func<BookRent, bool>> statusFilterExp;
                if (bookRentFilterViewModel.RentStatusFilter == RentStatusEnum.InRent)
                {
                    statusFilterExp = x => x.FactEndRent == null;
                }
                else
                {
                    statusFilterExp = x => x.PlanEndRent < DateTime.Now && x.FactEndRent == null;
                }
                filters.Add(statusFilterExp);
            }

            IQueryable<BookRent> query = _bookRent.GetAll();
            if (filters.Count > 0)
            {
                Expression<Func<BookRent, bool>> resultFilter = filters.OrTheseFiltersTogether();
                return Ok(query.Where(resultFilter).ToList());
            }
            
            return Ok(query.ToList());
        }

        /// <summary>
        /// Взять одну книгу в аренду определенным клиентом до указанной даты
        /// </summary>
        /// <param name="bookLeaseViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LeaseOne([FromBody] BookLeaseViewModel bookLeaseViewModel)
        {
            var book = await _bookStorage.GetAll().FirstOrDefaultAsync(x => x.BookId == bookLeaseViewModel.BookId);
            if (book == null)
            {
                throw new ValidationException("Данной книги нет в хранилище.");
            }

            if (_bookRent.GetAll().Any(x =>
                x.CustomerId == bookLeaseViewModel.CustomerId 
                && x.BookId == bookLeaseViewModel.BookId
                && !x.FactEndRent.HasValue))
            {
                throw new ValidationException("У клиента уже есть данная книга в аренде.");
            }

            if (book.Free == 0)
            {
                throw new ValidationException("Экземпляров книги больше нет в наличии.");
            }

            using var transaction = new CommittableTransaction(new TransactionOptions
                {IsolationLevel = IsolationLevel.ReadCommitted});
            try
            {
                book.Free--;
                await _bookStorage.SaveAsync(book);
                await _bookRent.SaveAsync(new BookRent
                {
                    BookId = bookLeaseViewModel.BookId,
                    CustomerId = bookLeaseViewModel.CustomerId,
                    StartRent = DateTime.Now,
                    PlanEndRent = bookLeaseViewModel.PlanEndDate,
                });
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                // log error
                throw e;
            }

            return Ok();
        }

        /// <summary>
        /// Вернуть одну книгу из аренды
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ReleaseOne([FromBody] long id)
        {
            var bookRentEntity = await _bookRent.GetAsync(id);
            var bookStorageEntity =
                await _bookStorage.GetAll().FirstOrDefaultAsync(x => x.BookId == bookRentEntity.BookId);
            using var transaction = new CommittableTransaction(new TransactionOptions
                {IsolationLevel = IsolationLevel.ReadCommitted});
            try
            {
                bookStorageEntity.Free++;
                bookRentEntity.FactEndRent = DateTime.Now;
                await _bookStorage.SaveAsync(bookStorageEntity);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // log error
                transaction.Rollback();
                throw ex;
            }

            return Ok();
        }
    }
}