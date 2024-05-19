using PWO.Client.Models;
using PWO.Client.Models.Items;
using PWO.Client.Models.List;
using PWO.Client.Models.Share;
using PWO.Client.Services.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PWO.Client.Services.ToDoListItems
{
    public class ToDoListItemService
    {
        private readonly RequestService _requestService;

        public ToDoListItemService(RequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<bool> CreateToDoListItemAsync(ToDoListItemCreateDto inputItem)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolistitems";
                await _requestService.PostAsync<ToDoListItemCreateDto, object>(uri, inputItem);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ToDoListItemReadDto> GetToDoListItemByIdAsync(int id)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolistitems/{id}";
                var item = await _requestService.GetAsync<ToDoListItemReadDto>(uri);
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<ToDoListItemReadDto>> GetToDoListItemsAsync(int toDoListId)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolists/{toDoListId}/items";
                var items = await _requestService.GetAsync<List<ToDoListItemReadDto>>(uri);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateToDoListItemAsync(int id, ToDoListItemUpdateDto inputItem)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolistitems/{id}";
                await _requestService.PutAsync<ToDoListItemUpdateDto, object>(uri, inputItem);
                return true;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<bool> DeleteToDoListItemAsync(int id)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolistitems/{id}";
                await _requestService.DeleteAsync(uri);
                return true;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }
    }
}