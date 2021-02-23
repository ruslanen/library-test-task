using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using library_test_task.Data;
using library_test_task.Data.Models;
using library_test_task.ViewModels;

namespace library_test_task.Validators
{
    public class BookRentValidator
    {
        private readonly IRepository<BookRent> _bookRent;

        public BookRentValidator(IRepository<BookRent> bookRent)
        {
            _bookRent = bookRent;
        }

        public void AfterLeaseValidate(BookStorage storableBook, BookLeaseViewModel bookLeaseViewModel)
        {
            if (storableBook == null)
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

            if (storableBook.Free == 0)
            {
                throw new ValidationException("Экземпляров книги больше нет в наличии.");
            }
        }
    }
}