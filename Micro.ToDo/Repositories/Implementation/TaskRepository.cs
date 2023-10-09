using Micro.Domain.Data;
using Micro.Domain.Dto_s;
using Microsoft.EntityFrameworkCore;

namespace Micro.ToDo.Repositories.Implementation;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context) => _context = context;

    public async System.Threading.Tasks.Task<List<TaskDto>> GetAllTaskByUserId(int userId)
    {
        var taskDtos = await _context.Tasks.Include(t => t.User).Select(t => new TaskDto()
        {
            Title = t.Title,
            Description = t.Description,
            Deadline = t.Deadline,
            Status = t.Status,
            UserId = t.User.Id
        }).ToListAsync();
        return taskDtos;
    }

    public async System.Threading.Tasks.Task UpdateTask(int taskId, TaskDto taskDto)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task != null)
        {
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.Status = taskDto.Status;
            task.Deadline = taskDto.Deadline;
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    public async System.Threading.Tasks.Task DeleteTask(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<TaskDto> CreateTask(TaskDto taskDto)
    {
        var task = new Micro.Domain.Entities.Task();
        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.Deadline = taskDto.Deadline;
        task.Status = task.Status;
        task.User = _context.Users.FirstOrDefault(u => u.Id == taskDto.UserId)!;
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return taskDto;
    }
}
