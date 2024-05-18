using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PWO.API.Models;

namespace PWO.API
{
    public class AppDbContext : IdentityDbContext<PWOUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoListItem> ToDoListItems { get; set; }
        public DbSet<ToDoListShare> ToDoListShares { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
