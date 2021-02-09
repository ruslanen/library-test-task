using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library_test_task.Data.Models
{
    /// <summary>
    /// Аренда книги
    /// </summary>
    public class BookRent : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        /// <summary>
        /// Книга
        /// </summary>
        public long BookId { get; set; }
        public virtual Book Book { get; set; }
        
        /// <summary>
        /// Дата начала аренды
        /// </summary>
        public DateTime StartRent { get; set; }
        
        /// <summary>
        /// Плановая дата окончания аренды
        /// </summary>
        public DateTime PlanEndRent { get; set; }
        
        /// <summary>
        /// Фактическая дата окончания аренды
        /// </summary>
        public DateTime? FactEndRent { get; set; }
    }
}