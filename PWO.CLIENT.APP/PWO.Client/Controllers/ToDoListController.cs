using PWO.Client.App_Start;
using PWO.Client.Models.List;
using PWO.Client.Models.Share;
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

    public class ToDoListController : Controller
    {
        private readonly ToDoListService _toDoListService;

        public ToDoListController(ToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var toDoLists = await _toDoListService.GetToDoListsAsync();
                return View(toDoLists);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Index"));
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
                if (toDoList == null)
                {
                    return HttpNotFound();
                }
                return View(toDoList);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Details"));
            }
        }

        public async Task<ActionResult> ShareUsers(int id)
        {
            try
            {
                var sharedUsers = await _toDoListService.GetToDoListSharesAsync(id);
                return View(sharedUsers);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        public async Task<ActionResult> Share(int id)
        {
            try
            {
                var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
                if (toDoList == null)
                {
                    return HttpNotFound();
                }

                ToDoListShareCreateDto dto = new ToDoListShareCreateDto
                {
                    ToDoListId = id,
                };
                return View(dto);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateToDoListShare(ToDoListShareCreateDto share)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _toDoListService.CreateToDoListShareAsync(share);

                    return RedirectToAction("Index");
                }

                var listToShow = await _toDoListService.GetToDoListByIdAsync(share.ToDoListId);
                return View(listToShow);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        public ActionResult Create()
        {
            try
            {
                ToDoListCreateDto listToCreate = new ToDoListCreateDto();
                return View(listToCreate);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoListCreateDto toDoList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _toDoListService.CreateToDoListAsync(toDoList);

                    return RedirectToAction("Index");
                }
                return View(toDoList);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
                if (toDoList == null)
                {
                    return HttpNotFound();
                }
                return View(toDoList);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ToDoListDetailsDto toDoList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ToDoListUpdateDto listToUpdate = new ToDoListUpdateDto()
                    {
                        IsCompleted = toDoList.IsCompleted,
                        Name = toDoList.Name,
                    };

                    await _toDoListService.UpdateToDoListAsync(id, listToUpdate);
                    return RedirectToAction("Index");
                }
                return View(toDoList);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
                if (toDoList == null)
                {
                    return HttpNotFound();
                }
                return View(toDoList);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _toDoListService.DeleteToDoListAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }
    }
}