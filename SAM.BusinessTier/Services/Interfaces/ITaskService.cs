using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface ITaskService
    {
        Task<Guid> CreateNewTask(CreateNewTaskRequest createNewTaskRequest);
        Task<bool> UpdateTask(Guid id, UpdateTaskRequest updateTaskRequest);
        Task<ICollection<GetTaskResponse>> GetTaskList(TaskFilter filter);
        Task<GetTaskResponse> GetTaskById(Guid id);
    }
}
