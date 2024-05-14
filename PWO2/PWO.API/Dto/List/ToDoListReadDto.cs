namespace PWO.API.Dto.List
{
    public class ToDoListReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
