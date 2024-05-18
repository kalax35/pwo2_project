using IO.Api.Dto.List;
using Microsoft.EntityFrameworkCore;
using PWO.API.Dto.Items;
using PWO.API.Dto.List;
using PWO.API.Models;
using System.Security.Claims;

namespace PWO.API.Endpoints
{
    public class NotificationEndpointBuilder
    {
        public static void RegisterEndpoints(ref WebApplication app)
        {
            app.MapGet("/notifications", async (AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notifications = await db.Notifications.Where(n => n.UserId == userId).ToListAsync();
                return Results.Ok(notifications);
            }).RequireAuthorization().WithOpenApi();

            app.MapPut("/notifications/{id}/read", async (int id, AppDbContext db, HttpContext httpContext) =>
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notification = await db.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

                if (notification == null)
                {
                    return Results.NotFound();
                }

                notification.IsRead = true;
                await db.SaveChangesAsync();

                return Results.Ok();
            }).RequireAuthorization().WithOpenApi();
        }
    }
}
