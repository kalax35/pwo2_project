using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PWO.Client.Models
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