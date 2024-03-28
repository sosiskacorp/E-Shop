using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop.Models.DTOs
{
    public class ClothingDisplayModel
    {
        public IEnumerable<Clothing> Clothings { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string SearchTerm { get; set; } = "";
        public int CategoryId { get; set; } = 0;
    }
}