namespace TaskApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation property: one user can have many tasks
        public List<TaskItem> Tasks { get; set;}  = new();
    }
}