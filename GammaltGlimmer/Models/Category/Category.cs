using System.Collections.Generic;

namespace GammaltGlimmer.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Item> Items { get; set; }
    }
}
