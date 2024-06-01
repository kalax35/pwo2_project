
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
                return Results.Ok(await db.ToDoListShares.Where(x => x.ToDoListId == id).Select(x => new ToDoListShareReadDto()
                {
                    Id = x.Id,
                    SharedUserName = x.User.UserName
                }).ToListAsync());
            }).RequireAuthorization().WithOpenApi();

            //    app.MapPost("/todolistshares", async (ToDoListShareCreateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            //    {
            //        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //        var toDoList = await db.ToDoLists.FindAsync(inputItem.ToDoListId);
            //        if (toDoList == null)
            //            return Results.NotFound();

            //        var userToShare = await db.Users.FirstOrDefaultAsync(u => u.UserName == inputItem.UserName);
            //        if (userToShare == null)
            //            return Results.NotFound();

            //        var existingShare = await db.ToDoListShares
            //.AnyAsync(s => s.ToDoListId == inputItem.ToDoListId && s.UserId == userToShare.Id);
            //        if (existingShare)
            //            return Results.Conflict(new { Message = "This user already has access to the to-do list." });


            //        //if (await db.Users.FirstOrDefaultAsync(x => x.UserName == inputItem.UserName) is PWOUser user)
            //        //{
            //        //    var item = new ToDoListShare()
            //        //    {
            //        //        Email = inputItem.UserName,
            //        //        ToDoListId = inputItem.ToDoListId,
            //        //        UserId = user.Id
            //        //    };

            //        //    db.ToDoListShares.Add(item);

            //        //    await db.SaveChangesAsync();

            //        //    var todoListShares = db.ToDoListShares.Where(x => x.Id == inputItem.ToDoListId).Select(x => x.UserId);

            //        //    if (!todoListShares.Contains(userId))
            //        //    {
            //        //        var item2 = new ToDoListShare()
            //        //        {
            //        //            Email = user.UserName,
            //        //            ToDoListId = inputItem.ToDoListId,
            //        //            UserId = user.Id
            //        //        };
            //        //        db.ToDoListShares.Add(item);
            //        //        await db.SaveChangesAsync();
            //        //    }

            //        var newShare = new ToDoListShare
            //        {
            //            UserId = userToShare.Id,
            //            ToDoListId = inputItem.ToDoListId,
            //            Email = inputItem.UserName
            //        };
            //        db.ToDoListShares.Add(newShare);
            //        await db.SaveChangesAsync();
            //        return Results.Created($"/todolistshares/{newShare.Id}", newShare);

            //    }).RequireAuthorization().WithOpenApi();

            app.MapPost("/todolistshares", async (ToDoListShareCreateDto inputItem, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var toDoList = await db.ToDoLists.FindAsync(inputItem.ToDoListId);
                if (toDoList == null)
                    return Results.NotFound();

                var userToShare = await db.Users.FirstOrDefaultAsync(u => u.UserName == inputItem.UserName);
                if (userToShare == null)
                    return Results.NotFound();

                var initiatorHasAccess = await db.ToDoListShares
                    .AnyAsync(s => s.ToDoListId == inputItem.ToDoListId && s.UserId == userId);

                if (!initiatorHasAccess)
                {
                    db.ToDoListShares.Add(new ToDoListShare
                    {
                        UserId = userId,
                        ToDoListId = inputItem.ToDoListId,
                        Email = httpContext.User.Identity.Name
                    });
                }

                var userAlreadyShared = await db.ToDoListShares
                    .AnyAsync(s => s.ToDoListId == inputItem.ToDoListId && s.UserId == userToShare.Id);

                if (userAlreadyShared)
                    return Results.Conflict(new { Message = "This user already has access to the to-do list." });

                db.ToDoListShares.Add(new ToDoListShare
                {
                    UserId = userToShare.Id,
                    ToDoListId = inputItem.ToDoListId,
                    Email = userToShare.UserName
                });

                await db.SaveChangesAsync();

                return Results.Created("/todolistshares", new { inputItem.ToDoListId, UserShared = userToShare.UserName, InitiatorAdded = !initiatorHasAccess });
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
