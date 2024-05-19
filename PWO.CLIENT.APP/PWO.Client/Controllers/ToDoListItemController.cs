using PWO.Client.Models.Items;
using PWO.Client.Models.List;
using PWO.Client.Services.ToDoListItems;
using PWO.Client.Services.ToDoLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PWO.Client.Controllers
{
    [Authorize]
    public class ToDoListItemController : Controller
    {
        private readonly ToDoListItemService _toDoListItemService;

        public ToDoListItemController(ToDoListItemService toDoListItemService)
        {
            _toDoListItemService = toDoListItemService;
        }

        public async Task<ActionResult> Index(int toDoListId)
        {
            var items = await _toDoListItemService.GetToDoListItemsAsync(toDoListId);
            return View(items);
        }

        public ActionResult Create(int toDoListId)
        {
            ViewBag.ToDoListId = toDoListId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoListItemCreateDto item)
        {
            if (ModelState.IsValid)
            {
                await _toDoListItemService.CreateToDoListItemAsync(item);
                return RedirectToAction("Index", new { toDoListId = item.ToDoListId });
            }
            return View(item);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var item = await _toDoListItemService.GetToDoListItemsAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ToDoListItemUpdateDto item)
        {
            if (ModelState.IsValid)
            {
                await _toDoListItemService.UpdateToDoListItemAsync(id, item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var item = await _toDoListItemService.GetToDoListItemsAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _toDoListItemService.DeleteToDoListItemAsync(id);
            return RedirectToAction("Index", new { toDoListId = id });
        }
    }
}