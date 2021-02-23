using System;
using System.ComponentModel.DataAnnotations;
using library_test_task.Data.Models;

namespace library_test_task.ViewModels
{
    /// <summary>
    /// Модель-преставления для добавления клиента
    /// </summary>
    public class CustomerViewModel
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
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