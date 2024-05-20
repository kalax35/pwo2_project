
using IO.Api.Dto.List;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PWO.API.Dto.Items;
using PWO.API.Dto.List;
using PWO.API.Hubs;
using PWO.API.Models;
using System.Security.Claims;

namespace PWO.API.Endpoints
{
    public class ToDoListEndpointBuilder
    {
        public static void RegisterEndpoints(ref WebApplication app)
        {
            //Get todolists
            app.MapGet("/todolists", async (HttpContext httpContext, AppDbContext db) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var sharedToMeIds = await db.ToDoListShares.Where(x => x.UserId == userId).Select(x => x.ToDoListId).ToListAsync();
                return (await db.ToDoLists.Where(x => x.CreationUserId == userId || sharedToMeIds.Contains(x.Id)).ToListAsync()).Select(x => new ToDoListReadDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreationTime = x.CreationTime,
                    IsCompleted = x.IsCompleted
                });
            }).RequireAuthorization().WithOpenApi();

            app.MapGet("/todolists/{id}", async (int id, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var item = await db.ToDoLists.FirstOrDefaultAsync(x => x.Id == id && x.CreationUserId == userId);

                if (item == null)
                {
                    var sharedToMeIds = await db.ToDoListShares.Where(x => x.UserId == userId).Select(x => x.ToDoListId).ToListAsync();

                    item = await db.ToDoLists.FirstOrDefaultAsync(x => sharedToMeIds.Contains(x.Id));
                    if (item == null)
                    {
                        return Results.NotFound();
                    }
                }

                return Results.Ok(new ToDoListDetailsDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationTime = item.CreationTime,
                    IsCompleted = item.IsCompleted,
                    Items = await db.ToDoListItems.Where(x => x.ToDoListId == id).Select(x => new ToDoListItemReadDto()
                    {
                        CreationTime = x.CreationTime,
                        Id = x.Id,
                        IsCompleted = x.IsCompleted,
                        Name = x.Name,
                        CompletionTime = x.CompletionTime,
                        CompletionUser = x.CompletionUser.UserName
                    }).ToListAsync()
                });
            }).RequireAuthorization().WithOpenApi();

            app.MapPost("/todolists", async (ToDoListCreateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var item = new ToDoList()
                {
                    Name = inputItem.Name,
                    CreationUserId = userId,
                    CreationTime = DateTime.Now
                };

                db.ToDoLists.Add(item);
                await db.SaveChangesAsync();
                return Results.Created($"/todoitems/{item.Id}", item);
            }).RequireAuthorization().WithOpenApi();

            //app.MapPut("/todolists/{id}", async (int id, ToDoListUpdateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            //{
            //    var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //    var item = await db.ToDoLists.FirstOrDefaultAsync(x => x.Id == id && x.CreationUserId == userId);

            //    if (item == null) return Results.NotFound();

            //    item.Name = inputItem.Name;
            //    if (inputItem.IsCompleted.HasValue)
            //    {
            //        item.IsCompleted = inputItem.IsCompleted.Value;
            //        if (inputItem.IsCompleted.Value)
            //        {
            //            item.CompletionTime = DateTime.Now;
            //        }
            //        else
            //        {
            //            item.CompletionTime = null;
            //        }
            //    }

            //    await db.SaveChangesAsync();

            //    return Results.NoContent();
            //}).RequireAuthorization().WithOpenApi();

            app.MapPut("/todolists/{id}", async (int id, ToDoListUpdateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var item = await db.ToDoLists.FirstOrDefaultAsync(x => x.Id == id && x.CreationUserId == userId);

                if (item == null) return Results.NotFound();

                item.Name = inputItem.Name;
                if (inputItem.IsCompleted.HasValue)
                {
                    item.IsCompleted = inputItem.IsCompleted.Value;
                    if (inputItem.IsCompleted.Value)
                    {
                        item.CompletionTime = DateTime.Now;

                        var notification = new Notification
                        {
                            UserId = userId,
                            Message = "Lista zadań została zakończona.",
                            CreatedAt = DateTime.Now,
                            IsSent = false,
                            IsRead = false
                        };
                        db.Notifications.Add(notification);
                    }
                    else
                    {
                        item.CompletionTime = null;
                    }
                }

                await db.SaveChangesAsync();
                return Results.NoContent();
            }).RequireAuthorization().WithOpenApi();

            app.MapDelete("/todolists/{id}", async (int id, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (await db.ToDoLists.FirstOrDefaultAsync(x => x.Id == id && x.CreationUserId == userId) is ToDoList item)
                {
                    db.ToDoLists.Remove(item);
                    await db.SaveChangesAsync();
                    return Results.Ok(item);
                }

                return Results.NotFound();
            }).RequireAuthorization().WithOpenApi();
        }
    }
}
