using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library_test_task.Data.Models
{
    /// <summary>
    /// Книга
    /// </summary>
    public class Book : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// Международный стандартный книжный номер (состоит из 13 цифр)
        /// </summary>
        public long Isbn { get; set; }
        
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Автор
        /// </summary>
        public string Author { get; set; }
        
        /// <summary>
        /// Год издания
        /// </summary>
        public int PublishYear { get; set; }
    }
}