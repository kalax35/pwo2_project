﻿using PWO.Client.App_Start;
using PWO.Client.Models;
using PWO.Client.Models.Items;
using PWO.Client.Models.List;
using PWO.Client.Services.ToDoListItems;
using PWO.Client.Services.ToDoLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PWO.Client.Controllers
{


    public class ToDoListItemController : Controller
    {
        private readonly ToDoListItemService _toDoListItemService;

        public ToDoListItemController(ToDoListItemService toDoListItemService)
        {
            _toDoListItemService = toDoListItemService;
        }


        public async Task<ActionResult> Details(int toDoListItemId, int listId)
        {
            try
            {
                var item = await _toDoListItemService.GetToDoListItemByIdAsync(toDoListItemId);
                item.listId = listId;
                return View(item);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        public ActionResult Create(int id)
        {
            try
            {
                ToDoListItemCreateDto itemToCreate = new ToDoListItemCreateDto()
                {
                    ToDoListId = id,
                };
                return View(itemToCreate);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoListItemCreateDto item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _toDoListItemService.CreateToDoListItemAsync(item);
                    return RedirectToAction("Details", "ToDoList", new { id = item.ToDoListId });
                }
                return View(item);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        public async Task<ActionResult> Edit(int id, int listId)
        {
            try
            {
                var item = await _toDoListItemService.GetToDoListItemByIdAsync(id);

                ToDoListItemListUpdateDto itemToUpdate = new ToDoListItemListUpdateDto()
                {
                    Id = id,
                    Name = item.Name,
                    listId = listId,
                    IsCompleted = item.IsCompleted,
                };
                return View(itemToUpdate);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ToDoListItemListUpdateDto item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ToDoListItemUpdateDto toSend = new ToDoListItemUpdateDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        IsCompleted = item.IsCompleted,
                    };
                    await _toDoListItemService.UpdateToDoListItemAsync(item.listId, toSend);
                    return RedirectToAction("Details", "ToDoList", new { id = item.listId });
                }
                return View(item);
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
                var item = await _toDoListItemService.GetToDoListItemByIdAsync(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                return View(item);
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
                await _toDoListItemService.DeleteToDoListItemAsync(id);
                return RedirectToAction("Index", "ToDoList");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ToDoList", "Share"));
            }
        }
    }
}