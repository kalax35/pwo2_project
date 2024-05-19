namespace PWO.Client.Models.Items
{
    public class ToDoListItemUpdateDto : ToDoListItemReadDto
    {
        public string Name { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
