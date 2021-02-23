using System;
using System.ComponentModel.DataAnnotations;

namespace library_test_task.ViewModels
{
    /// <summary>
    /// Модель-преставления для фиксации аренды книги
    /// </summary>
    public class BookLeaseViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long CustomerId { get; set; }
        
        [Required]
        [Range(1, long.MaxValue)]
        public long BookId { get; set; }
        
        [Required]
        public DateTime PlanEndDate { get; set; }
    }
}