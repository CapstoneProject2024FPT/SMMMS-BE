using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Payload.Task;

namespace SAM.API.Controllers
{
    [ApiController]
    public class TaskController : BaseController<TaskController>
    {
        readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService) : base(logger)
        {
            _taskService = taskService;
        }
        [HttpPost(ApiEndPointConstant.TaskManager.TasksEndPoint)]
        public async Task<IActionResult> CreateNewTask(CreateNewTaskRequest createNewTaskRequest)
        {
            var response = await _taskService.CreateNewTask(createNewTaskRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.TaskManager.TasksEndPoint)]
        public async Task<IActionResult> GetTaskList([FromQuery] TaskFilter filter)
        {
            var response = await _taskService.GetTaskList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.TaskManager.TaskEndPoint)]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var response = await _taskService.GetTaskById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.TaskManager.TaskEndPoint)]
        public async Task<IActionResult> UpdateRank(Guid id, UpdateTaskRequest updateTaskRequest)
        {
            var isSuccessful = await _taskService.UpdateTask(id, updateTaskRequest);
            if (!isSuccessful) return Ok(MessageConstant.TaskManager.UpdateTaskFailedMessage);
            return Ok(MessageConstant.TaskManager.UpdateTaskSuccessMessage);
        }


    }
    
}
