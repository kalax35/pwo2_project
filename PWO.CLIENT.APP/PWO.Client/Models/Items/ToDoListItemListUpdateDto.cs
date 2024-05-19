using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWO.Client.Models.Items
{
    public class ToDoListItemListUpdateDto : ToDoListItemUpdateDto
    {
        public int Id { get; set; }
        public int listId { get; set; }
    }
}