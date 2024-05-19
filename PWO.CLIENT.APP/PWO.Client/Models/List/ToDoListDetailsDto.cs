using PWO.Client.Models.Items;
using System.Collections.Generic;

namespace PWO.Client.Models.List
{
    public class ToDoListDetailsDto : ToDoListItemReadDto
    {
        public List<ToDoListItemReadDto> Items { get; set; }
    }
}
