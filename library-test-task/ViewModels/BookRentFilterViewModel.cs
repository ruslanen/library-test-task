namespace library_test_task.ViewModels
{
    /// Модель-преставления фильтрации книг в аренде
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
        All = 0,
        InRent,
        RentExpired, 
    }
}