using System.Threading.Tasks;
using library_test_task.Services;
using library_test_task.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace library_test_task.Controllers
{
    public class BookRentController : Controller
    {
        private readonly BookRentService _bookRentService;
        
        public BookRentController(BookRentService bookRentService)
        {
            _bookRentService = bookRentService;
        }

        /// <summary>
        /// Получить последовательность аренд книг
        /// </summary>
        /// <param name="bookRentFilterViewModel">Модель-представления с данными о фильтрации</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List(BookRentFilterViewModel bookRentFilterViewModel) =>
            Ok(await _bookRentService.GetBookRentsAsync(bookRentFilterViewModel));

        /// <summary>
        /// Взять одну книгу в аренду определенным клиентом до указанной даты
        /// </summary>
        /// <param name="bookLeaseViewModel">Модель-преставления аренды книги</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LeaseOne([FromBody] BookLeaseViewModel bookLeaseViewModel)
        {
            await _bookRentService.LeaseBookAsync(bookLeaseViewModel);
            return Ok();
        }

        /// <summary>
        /// Вернуть одну книгу из аренды
        /// </summary>
        /// <param name="id">Идентификатор операции аренды</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ReleaseOne([FromBody] long id)
        {
            await _bookRentService.ReleaseBookAsync(id);
            return Ok();
        }
    }
}