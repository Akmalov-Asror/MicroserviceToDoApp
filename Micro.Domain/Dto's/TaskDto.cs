using System.Net.NetworkInformation;
using Micro.Domain.Enums;

namespace Micro.Domain.Dto_s;

public class TaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public EStatus Status { get; set; }
    public int UserId { get; set; }
}