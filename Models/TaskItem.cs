namespace TaskApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        // foreign key : UserId
        public int UserId { get; set; }

        //Navigation property: many tasks belong to one user
        public User User { get; set; }
    }
}
