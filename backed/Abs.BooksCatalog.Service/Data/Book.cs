using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Abs.BooksCatalog.Service.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public DateTime? PublishedOn { get; set; }
        public List<Author> Authors { get; set; }
        public List<Cover> Covers { get; set; }
    }
}