using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library_test_task.Data.Models
{
    /// <summary>
    /// Хранилище книг
    /// </summary>
    public class BookStorage : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// Книга
        /// </summary>
        public long BookId { get; set; }
        public virtual Book Book { get; set; }
        
        /// <summary>
        /// Общее количество экземпляров текущей книги
        /// </summary>
        public int Total { get; set; }
        
        /// <summary>
        /// Количество свободных экземпляров книги 
        /// </summary>
        public int Free { get; set; }
    }
}