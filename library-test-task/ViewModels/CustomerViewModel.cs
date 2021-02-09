using System;
using library_test_task.Data.Models;

namespace library_test_task.ViewModels
{
    /// <summary>
    /// Модель-преставления для добавления клиента
    /// </summary>
    public class CustomerViewModel
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        public Customer ToCustomer() => new Customer
        {
            LastName = this.LastName,
            FirstName = this.FirstName,
            Patronymic = this.Patronymic,
            BirthDate = this.BirthDate,
            Address = this.Address,
        };
    }
}