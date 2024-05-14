namespace PWO.API.Dto.List
{
    public class ToDoListUpdateDto : ToDoListCreateDto
    {

        public bool? IsCompleted { get; set; }
    }
}
