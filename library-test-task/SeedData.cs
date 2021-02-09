using System;
using System.Linq;
using library_test_task.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace library_test_task
{
    /// <summary>
    /// Заполняет базу данных тестовыми данными
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Заполнить таблицы <see cref="Book"/> и <see cref="Customer"/>
        /// </summary>
        /// <param name="applicationBuilder"></param>
        public static void EnsurePopulated(IApplicationBuilder applicationBuilder)
        {
            DbContext context = applicationBuilder.ApplicationServices
                    .CreateScope().ServiceProvider.GetRequiredService<DbContext>();
            var books = context.Set<Book>();
            var customers = context.Set<Customer>();
            var bookStorage = context.Set<BookStorage>();
            context.Database.Migrate();
            if (!books.Any())
            {
                books.AddRange(
                    new Book
                    {
                        Isbn = 9785496004336,
                        Title = "CLR via C#",
                        Author = "Jeffrey Richter",
                        PublishYear = 2012,
                    },
                    new Book
                    {
                        Isbn = 9781492054504,
                        Title = "Concurrency in C#",
                        Author = "Stephen Cleary",
                        PublishYear = 2019,
                    },
                    new Book
                    {
                        Isbn = 9781484254394,
                        Title = "Pro ASP.NET Core 3",
                        Author = "Adam Freeman",
                        PublishYear = 2019,
                    },
                    new Book
                    {
                        Isbn = 9781484257555,
                        Title = "Pro C# 8 with .NET Core 3",
                        Author = "Adam Freeman",
                        PublishYear = 2020,
                    },
                    new Book
                    {
                        Isbn = 9781593279523,
                        Title = "The Linux Command Line, 2nd Edition",
                        Author = "William Shotts",
                        PublishYear = 2019,
                    }
                );
                context.SaveChanges();
            }

            if (!bookStorage.Any())
            {
                bookStorage.AddRange(
                    new BookStorage
                    {
                        BookId = 1,
                        Total = 3,
                        Free = 3,
                    },
                    new BookStorage
                    {
                        BookId = 2,
                        Total = 2,
                        Free = 2,
                    },
                    new BookStorage
                    {
                        BookId = 3,
                        Total = 1,
                        Free = 1,
                    },
                    new BookStorage
                    {
                        BookId = 4,
                        Total = 3,
                        Free = 3,
                    },
                    new BookStorage
                    {
                        BookId = 5,
                        Total = 2,
                        Free = 2,
                    });
                context.SaveChanges();
            }

            if (!customers.Any())
            {
                customers.AddRange(
                    new Customer
                    {
                        LastName = "Петров",
                        FirstName = "Федор",
                        Patronymic = "Алексеевич",
                        BirthDate = new DateTime(1990, 10, 11),
                        Address = "Казань, ул. Восстания 112, д. 5, кв. 22",
                    },
                    new Customer
                    {
                        LastName = "Иванов",
                        FirstName = "Андрей",
                        Patronymic = "Юрьевич",
                        BirthDate = new DateTime(1982, 4, 28),
                        Address = "Казань, ул. Дементьева 10, д. 83, кв. 122",
                    },
                    new Customer
                    {
                        LastName = "Павлова",
                        FirstName = "Елена",
                        Patronymic = "Дмитриевна",
                        BirthDate = new DateTime(1994, 12, 10),
                        Address = "Казань, ул. Ибрагимова 5, д. 13, кв. 25",
                    },
                    new Customer
                    {
                        LastName = "Алексеева",
                        FirstName = "Надежда",
                        Patronymic = "Викторовна",
                        BirthDate = new DateTime(1996, 3, 4),
                        Address = "Казань, ул. Чуйкова 38, д. 1, кв. 34",
                    },
                    new Customer
                    {
                        LastName = "Афанасьев",
                        FirstName = "Кирилл",
                        Patronymic = "Андреевич",
                        BirthDate = new DateTime(1994, 4, 23),
                        Address = "Казань, ул. Мусина 34, д. 34, кв. 43",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}