using System.ComponentModel.DataAnnotations;

namespace Abs.BooksCatalog.Service.Data
{
    public class Author 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}