namespace PWO.API.Dto.Items
{
    public class ToDoListItemReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string CompletionUser { get; set; }
    }
}
