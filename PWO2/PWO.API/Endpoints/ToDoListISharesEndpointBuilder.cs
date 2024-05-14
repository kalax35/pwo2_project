
using Microsoft.EntityFrameworkCore;
using PWO.API.Dto.Share;
using PWO.API.Models;
using System.Security.Claims;

namespace PWO.API.Endpoints
{
    public class ToDoListISharesEndpointBuilder
    {
        public static void RegisterEndpoints(ref WebApplication app)
        {
            app.MapGet("/todolistshares/{id}", async (int id, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Results.Ok(await db.ToDoListShares.Where(x => x.ToDoListId == id && x.ToDoList.CreationUserId == userId).Select(x => new ToDoListShareReadDto()
                {
                    Id = x.Id,
                    SharedUserName = x.User.UserName
                }).ToListAsync());
            }).RequireAuthorization().WithOpenApi();

            app.MapPost("/todolistshares", async (ToDoListShareCreateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!(await db.ToDoLists.AnyAsync(x => x.Id == inputItem.ToDoListId && x.CreationUserId == userId)))
                {
                    if (!(await db.ToDoListShares.AnyAsync(x => x.UserId == userId && x.ToDoListId == inputItem.ToDoListId)))
                    {
                        return Results.NotFound();
                    }
                }

                if (await db.Users.FirstOrDefaultAsync(x => x.UserName == inputItem.UserName) is PWOUser user)
                {
                    var item = new ToDoListShare()
                    {
                        Email = inputItem.UserName,
                        ToDoListId = inputItem.ToDoListId,
                        UserId = user.Id
                    };

                    db.ToDoListShares.Add(item);
                    await db.SaveChangesAsync();
                    return Results.Created($"/todolistshares/{item.Id}", item);
                }
                return Results.NotFound();
            }).RequireAuthorization().WithOpenApi();

            app.MapDelete("/todolistshares/{id}", async (int id, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (await db.ToDoListShares.FirstOrDefaultAsync(x => x.Id == id && x.ToDoList.CreationUserId == userId) is ToDoListShare item)
                {
                    db.ToDoListShares.Remove(item);
                    await db.SaveChangesAsync();
                    return Results.Ok(item);
                }

                return Results.NotFound();
            }).RequireAuthorization().WithOpenApi();
        }
    }
}
