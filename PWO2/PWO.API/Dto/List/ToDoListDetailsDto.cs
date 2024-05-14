using PWO.API.Dto.Items;

namespace IO.Api.Dto.List
{
    public class ToDoListDetailsDto : ToDoListItemReadDto
    {
        public List<ToDoListItemReadDto> Items { get; set; }
    }
}
