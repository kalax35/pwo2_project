using PWO.Client.Models;
using PWO.Client.Models.List;
using PWO.Client.Models.Share;
using PWO.Client.Services.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PWO.Client.Services.ToDoLists
{
    public class ToDoListService
    {
        private readonly RequestService _requestService;

        public ToDoListService(RequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<List<ToDoListReadDto>> GetToDoListsAsync()
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolists";
                var response = await _requestService.GetAsync<List<ToDoListReadDto>>(uri);
                return response;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<ToDoListDetailsDto> GetToDoListByIdAsync(int id)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolists/{id}";
                var response = await _requestService.GetAsync<ToDoListDetailsDto>(uri);
                return response;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<ToDoList> CreateToDoListAsync(ToDoListCreateDto inputItem)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolists";
                var response = await _requestService.PostAsync<ToDoListCreateDto, ToDoList>(uri, inputItem);
                return response;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<bool> UpdateToDoListAsync(int id, ToDoListUpdateDto inputItem)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolists/{id}";
                await _requestService.PutAsync<ToDoListUpdateDto>(uri, inputItem);
                return true;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<bool> DeleteToDoListAsync(int id)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolists/{id}";
                await _requestService.DeleteAsync(uri);
                return true;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<List<ToDoListShareReadDto>> GetToDoListSharesAsync(int id)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolistshares/{id}";
                var response = await _requestService.GetAsync<List<ToDoListShareReadDto>>(uri);
                return response;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<bool> CreateToDoListShareAsync(ToDoListShareCreateDto inputItem)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolistshares";
                await _requestService.PostAsync<ToDoListShareCreateDto>(uri, inputItem);
                return true;
            }
            catch (Exception ex)
            {
 
                throw ex;
            }
        }

        public async Task<bool> DeleteToDoListShareAsync(int id)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/todolistshares/{id}";
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