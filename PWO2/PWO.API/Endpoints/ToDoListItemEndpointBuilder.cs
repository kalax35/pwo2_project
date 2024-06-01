
using IO.Api.Dto.List;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PWO.API.Dto.Items;
using PWO.API.Models;
using System.Net.Http;
using System.Security.Claims;

namespace PWO.API.Endpoints
{
    public class ToDoListItemEndpointBuilder
    {
        public static void RegisterEndpoints(ref WebApplication app)
        {
            app.MapGet("/todolistitems/{id}", async (int id, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var item = await db.ToDoListItems.FirstOrDefaultAsync(x => x.Id == id);

                if (!(await db.ToDoLists.AnyAsync(x => x.Id == id)))
                {
                    if (!(await db.ToDoListShares.AnyAsync(x => x.UserId == userId)))
                    {
                        return Results.NotFound();
                    }
                }

                return Results.Ok(new ToDoListItem()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationTime = item.CreationTime,
                    IsCompleted = item.IsCompleted,
                });
            }).RequireAuthorization().WithOpenApi();

            app.MapPost("/todolistitems", async (ToDoListItemCreateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!(await db.ToDoLists.AnyAsync(x => x.Id == inputItem.ToDoListId)))
                {
                    if (!(await db.ToDoListShares.AnyAsync(x => x.UserId == userId && x.ToDoListId == inputItem.ToDoListId)))
                    {
                        return Results.NotFound();
                    }
                }
                var toDoList = await db.ToDoLists.FirstOrDefaultAsync(x => x.Id == inputItem.ToDoListId);
                var item = new ToDoListItem()
                {
                    Name = inputItem.Name,
                    CreationUserId = userId,
                    CreationTime = DateTime.Now,
                    ToDoListId = inputItem.ToDoListId,
                };

                var sharedUsers = await db.ToDoListShares
                               .Where(x => x.ToDoListId == inputItem.ToDoListId)
                               .Select(x => x.Email)
                               .ToListAsync();

                foreach (var sharedUserId in sharedUsers)
                {
                    var notification = new Notification
                    {
                        UserId = sharedUserId,
                        Message = $"Nowy element {item.Name} został dodany do listy zadań {toDoList.Name}",
                        CreatedAt = DateTime.Now,
                        IsSent = false,
                        IsRead = false
                    };
                    db.Notifications.Add(notification);
                }

                db.ToDoListItems.Add(item);
                await db.SaveChangesAsync();
                return Results.Created($"/todoitems/{item.Id}", item);
            }).RequireAuthorization().WithOpenApi();

            app.MapPut("/todolistitems/{id}", async (int id, ToDoListItemUpdateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var item = await db.ToDoListItems.FirstOrDefaultAsync(x => x.ToDoListId == id && x.Id == inputItem.Id);

                if (item == null)
                {
                    var sharedToMeIds = await db.ToDoListShares.Where(x => x.UserId == userId).Select(x => x.ToDoListId).ToListAsync();

                    item = await db.ToDoListItems.FirstOrDefaultAsync(x => x.Id == id && sharedToMeIds.Contains(x.ToDoListId));
                    if (item == null)
                    {
                        return Results.NotFound();
                    }
                }


                item.Name = inputItem.Name;
                if (inputItem.IsCompleted.HasValue)
                {
                    item.IsCompleted = inputItem.IsCompleted.Value;
                    if (inputItem.IsCompleted.Value)
                    {
                        item.CompletionTime = DateTime.Now;
                        item.CompletionUserId = userId;

                        var sharedUsers = await db.ToDoListShares
                            .Where(x => x.ToDoListId == id)
                            .Select(x => x.Email)
                            .ToListAsync();

                        foreach (var sharedUserId in sharedUsers)
                        {
                            var notification = new Notification
                            {
                                UserId = sharedUserId,
                                Message = $"Element {item.Name} został oznaczony jako ukończony.",
                                CreatedAt = DateTime.Now,
                                IsSent = false,
                                IsRead = false
                            };
                            db.Notifications.Add(notification);
                        }
                    }
                    else
                    {
                        item.CompletionTime = null;
                        item.CompletionUserId = null;
                    }
                }

                await db.SaveChangesAsync();

                return Results.NoContent();
            }).RequireAuthorization().WithOpenApi();

            app.MapDelete("/todolistitems/{id}", async (int id, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (await db.ToDoListItems.FirstOrDefaultAsync(x => x.Id == id) is ToDoListItem item)
                {
                    db.ToDoListItems.Remove(item);
                    await db.SaveChangesAsync();
                    return Results.Ok(item);
                }
                else
                {
                    var sharedToMeIds = await db.ToDoListShares.Where(x => x.UserId == userId).Select(x => x.ToDoListId).ToListAsync();

                    if (await db.ToDoListItems.FirstOrDefaultAsync(x => x.Id == id && sharedToMeIds.Contains(x.ToDoListId)) is ToDoListItem item2)
                    {
                        db.ToDoListItems.Remove(item2);
                        await db.SaveChangesAsync();
                        return Results.Ok(item2);
                    }
                }
                return Results.NotFound();
            }).RequireAuthorization().WithOpenApi();
        }
    }
}
