using System.ComponentModel.DataAnnotations.Schema;

namespace PWO.API.Models
{
    public class ToDoListItem
    {
        public int Id { get; set; }

        public int ToDoListId { get; set; }
        [ForeignKey(nameof(ToDoListId))]
        public ToDoList ToDoList { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CompletionTime { get; set; }

        public DateTime CreationTime { get; set; }


        public string CreationUserId { get; set; }
        [ForeignKey(nameof(CreationUserId))]
        public PWOUser CreationUser { get; set; }


        public string? CompletionUserId { get; set; }
        [ForeignKey(nameof(CompletionUserId))]
        public PWOUser CompletionUser { get; set; }
    }
}
