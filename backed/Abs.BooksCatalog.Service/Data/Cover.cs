using System.ComponentModel.DataAnnotations;

namespace Abs.BooksCatalog.Service.Data
{
    public class Cover
    {
        [Key]
        public string Code { get; set; }
    }
}
