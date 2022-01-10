using GammaltGlimmer.Data;
using GammaltGlimmer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GammaltGlimmer.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemController(IItemRepository itemRepository, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _itemRepository = itemRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "Auctioneer")]
        public IActionResult List()
        {
            return View(_itemRepository.AllItems);
        }
        [Authorize]
        public ActionResult ListPurchaser()
        {
            var currentuser = _httpContextAccessor.HttpContext.User.Identity.Name;
            var items = _itemRepository.AllItems;
            items = items.Where(c => c.CreatedBy == currentuser);
            return View(items);
        }
        [Authorize]
        public IActionResult ItemIdExist()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View(new Item());
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ItemId,Name,Description,ReleaseYear,StartPrice,CategoryId,CreatedBy")] Item item)
        {
            item.CreatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            item.Status = "Kvar i lager";
            if (ModelState.IsValid)
            {
                var itemexist = _context.Items.FirstOrDefault(d => d.ItemId == item.ItemId);
                if (itemexist == null)
                {
                    _itemRepository.Add(item);
                    _itemRepository.Save();

                    bool userCoordinator = User.IsInRole("Auctioneer");
                    if (userCoordinator)
                    {
                        return RedirectToAction("List", "Item");
                    }
                    else
                    {
                        return RedirectToAction("ListPurchaser", "Item");
                    }
                }
                else
                {
                    return RedirectToAction("ItemIdExist", "Item");
                }
            }
            return View(item);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Edit(string id)
        {
            Item item = _itemRepository.GetItemById(id);
            return View(item);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _itemRepository.Edit(item);
                _itemRepository.Save();
                bool userCoordinator = User.IsInRole("Auctioneer");
                if (userCoordinator)
                {
                    return RedirectToAction("List", "Item");
                }
                else
                {
                    return RedirectToAction("ListPurchaser", "Item");
                }
            }
            return View(item);
        }
        [Authorize(Roles = "Auctioneer")]
        public ActionResult Delete(string id)
        {
            Item item = _itemRepository.GetItemById(id);

            if (item == null)
                return View("Not Found");
            else
                return View(item);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Auctioneer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _itemRepository.Remove(id);
            _itemRepository.Save();
            return RedirectToAction("List","Item");
        }
    }
}
