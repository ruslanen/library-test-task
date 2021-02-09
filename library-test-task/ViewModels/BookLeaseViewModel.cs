using System;

namespace library_test_task.ViewModels
{
    /// Модель-преставления для фиксации аренды книги
    public class BookLeaseViewModel
    {
        public long CustomerId { get; set; }
        
        public long BookId { get; set; }
        
        public DateTime PlanEndDate { get; set; }
    }
}