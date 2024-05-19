namespace PWO.Client.Models.List
{
    public class ToDoListUpdateDto : ToDoListCreateDto
    {

        public bool? IsCompleted { get; set; }
    }
}
