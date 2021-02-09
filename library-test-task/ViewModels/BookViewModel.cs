using library_test_task.Data.Models;

namespace library_test_task.ViewModels
{
    /// <summary>
    /// Модель-преставления для добавления новой книги в хранилище
    /// </summary>
    public class BookViewModel
    {
        public long Isbn { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int PublishYear { get; set; }
        
        public int Total { get; set; }

        public Book ToBook() => new Book
        {
            Isbn = this.Isbn,
            Title = this.Title,
            Author = this.Author,
            PublishYear = this.PublishYear,
        };
    }
}