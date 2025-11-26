namespace TaskApi.DTOs
{
    public class UserWithTasksDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<TaskDto>? Tasks { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
