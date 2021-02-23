using System.ComponentModel.DataAnnotations;
using library_test_task.Data.Models;

namespace library_test_task.ViewModels
{
    /// <summary>
    /// Модель-преставления для добавления новой книги в хранилище
    /// </summary>
    public class BookViewModel
    {
        [Required]
        [RegularExpression("^[0-9]{13}$")]
        public long Isbn { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int PublishYear { get; set; }
        
        [Required]
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