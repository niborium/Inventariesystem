using System.Collections.Generic;

namespace GammaltGlimmer.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> AllCategories { get; }
    }
}
