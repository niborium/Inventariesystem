using GammaltGlimmer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GammaltGlimmer.Models
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        public ItemRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Item> AllItems
        {
            get
            {
                return _appDbContext.Items.Include(c => c.Category);
            }
        }
        public Item GetItemById(string itemId)
        {
            return _appDbContext.Items.FirstOrDefault(p => p.ItemId == itemId);
        }
        public void Add(Item item)
        {
            _appDbContext.Items.Add(item);
            Save();
        }
        public void Edit(Item item)
        {
            _appDbContext.Entry(item).State = EntityState.Modified;
        }
        public void Remove(string id)
        {
            var item = _appDbContext.Items.Find(id);
            if (item != null) _appDbContext.Items.Remove(item);
        }
        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
