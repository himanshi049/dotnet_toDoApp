using TaskApi.DTOs;
using TaskApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace TaskApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();
        private int _nextId = 1;

        public List<TaskItem> GetAll()
        {
            return _tasks;
        }

        public TaskItem GetById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public TaskItem Create(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Id = dto.Id ?? _nextId++,
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };
            if (dto.Id.HasValue && dto.Id.Value >= _nextId)
            {
                _nextId = dto.Id.Value + 1;
            }
            _tasks.Add(task);
            return task;
        }

        public TaskItem Update(int id, CreateTaskDto dto)
        {
            var task = GetById(id);
            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description;
            return task;
        }

        public bool ToggleComplete(int id)
        {
            var task = GetById(id);
            if (task == null) return false;

            task.IsCompleted = !task.IsCompleted;
            return true;
        }

        public bool Delete(int id)
        {
            var task = GetById(id);
            if (task == null) return false;

            _tasks.Remove(task);
            return true;
        }
    }
}
