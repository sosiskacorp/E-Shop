using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Shop.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public List<Clothing> Clothings { get; set; }
    }
}
