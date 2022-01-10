using System.Collections.Generic;

namespace GammaltGlimmer.Models
{
    public interface IItemRepository
    {
        IEnumerable<Item> AllItems { get; }
        Item GetItemById(string itemId);
        void Add(Item item);
        void Edit(Item item);
        void Remove(string id);
        void Save();
    }
}
