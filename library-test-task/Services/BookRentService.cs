using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using library_test_task.Data;
using library_test_task.Data.Models;
using library_test_task.Validators;
using library_test_task.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace library_test_task.Services
{
    public class BookRentService
    {
        private readonly IRepository<BookStorage> _bookStorage;
        private readonly IRepository<BookRent> _bookRent;
        private readonly BookRentValidator _bookRentValidator;

        public BookRentService(
            IRepository<BookStorage> bookStorage,
            IRepository<BookRent> bookRent,
            BookRentValidator bookRentValidator)
        {
            _bookStorage = bookStorage;
            _bookRent = bookRent;
            _bookRentValidator = bookRentValidator;
        }
        
        public async Task<IEnumerable<BookRent>> GetBookRentsAsync(BookRentFilterViewModel bookRentFilterViewModel)
        {
            var filters = new List<Expression<Func<BookRent, bool>>>();
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
                    // RentStatusEnum.RentExpired
                    statusFilterExp = x => x.PlanEndRent < DateTime.Now && x.FactEndRent == null;
                }
                
                filters.Add(statusFilterExp);
            }

            IQueryable<BookRent> query = _bookRent.GetAll();
            if (filters.Count > 0)
            {
                Expression<Func<BookRent, bool>> resultFilter = filters.OrTheseFiltersTogether();
                return await query.Where(resultFilter).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task LeaseBookAsync(BookLeaseViewModel bookLeaseViewModel)
        {
            var storableBook = await _bookStorage.GetAll().FirstOrDefaultAsync(x => x.BookId == bookLeaseViewModel.BookId);
            _bookRentValidator.AfterLeaseValidate(storableBook, bookLeaseViewModel);
            
            using var transaction = new CommittableTransaction(new TransactionOptions
                {IsolationLevel = IsolationLevel.ReadCommitted});
            try
            {
                storableBook.Free--;
                await _bookStorage.SaveAsync(storableBook);
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
        }

        public async Task ReleaseBookAsync(long id)
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
        }
    }
}