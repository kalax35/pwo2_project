using PWO.Client.Models.List;
using PWO.Client.Services.ToDoLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PWO.Client.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ToDoListService _toDoListService;

        public ToDoListController(ToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        public async Task<ActionResult> Index()
        {
            var toDoLists = await _toDoListService.GetToDoListsAsync();
            return View(toDoLists);
        }

        public async Task<ActionResult> Details(int id)
        {
            var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            return View(toDoList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoListCreateDto toDoList)
        {
            if (ModelState.IsValid)
            {
                await _toDoListService.CreateToDoListAsync(toDoList);
                return RedirectToAction("Index");
            }
            return View(toDoList);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            return View(toDoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ToDoListUpdateDto toDoList)
        {
            if (ModelState.IsValid)
            {
                await _toDoListService.UpdateToDoListAsync(id, toDoList);
                return RedirectToAction("Index");
            }
            return View(toDoList);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            return View(toDoList);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _toDoListService.DeleteToDoListAsync(id);
            return RedirectToAction("Index");
        }
    }
}