using TaskApi.DTOs;
using TaskApi.Models;
using System.Collections.Generic;

namespace TaskApi.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAll();
        TaskItem GetById(int id);
        TaskItem Create(CreateTaskDto dto);
        TaskItem Update(int id, CreateTaskDto dto);
        bool ToggleComplete(int id);
        bool Delete(int id);
    }
}
