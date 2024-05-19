namespace PWO.Client.Models.Items
{
    public class ToDoListItemUpdateDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}
