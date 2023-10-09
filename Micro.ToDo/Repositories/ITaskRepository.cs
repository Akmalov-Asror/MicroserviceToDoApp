using Micro.Domain.Dto_s;

namespace Micro.ToDo.Repositories;

public interface ITaskRepository
{
    System.Threading.Tasks.Task<List<TaskDto>> GetAllTaskByUserId(int userId);

    System.Threading.Tasks.Task UpdateTask(int taskId, TaskDto taskDto);

    System.Threading.Tasks.Task DeleteTask(int taskId);

    System.Threading.Tasks.Task<TaskDto> CreateTask(TaskDto taskDto);

}