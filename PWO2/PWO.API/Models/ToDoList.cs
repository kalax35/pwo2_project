using System.ComponentModel.DataAnnotations.Schema;

namespace PWO.API.Models
{
    public class ToDoList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletionTime { get; set; }

        public DateTime CreationTime { get; set; }
        public string CreationUserId { get; set; }
        [ForeignKey(nameof(CreationUserId))]
        public PWOUser CreationUser { get; set; }
    }
}
