namespace library_test_task.ViewModels
{
    /// <summary>
    /// Модель-преставления для фильтрации книг в аренде
    /// </summary>
    public class BookRentFilterViewModel
    {
        public string NameFilter { get; set; }
        
        public string BookFilter { get; set; }
        
        public RentStatusEnum RentStatusFilter { get; set; }
    }

    /// <summary>
    /// Статусы фильтрации аренды книг
    /// </summary>
    public enum RentStatusEnum
    {
        // Все книги
        All = 0,
        // Только те книги, которые на данный момент в аренде
        InRent,
        // Только те книги, у которых просрочено время аренды
        RentExpired, 
    }
}