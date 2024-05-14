using System.ComponentModel.DataAnnotations.Schema;

namespace PWO.API.Models
{
    public class ToDoListShare
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int ToDoListId { get; set; }
        [ForeignKey(nameof(ToDoListId))]
        public ToDoList ToDoList { get; set; }

        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public PWOUser User { get; set; }
    }
}
