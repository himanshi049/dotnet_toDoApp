namespace TaskApi.DTOs
{
    public class CreateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int? UserId { get; set; }
    }
}
