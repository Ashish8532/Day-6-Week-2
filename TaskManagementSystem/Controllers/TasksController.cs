using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementSystem.DataAccess.Repository.IRepository;
using TaskManagementSystem.Models.Authentication;
using TaskManagementSystem.Models.Models;
using TaskManagementSystem.Models.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TasksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _unitOfWork.Tasks.GetAll();
                var taskVm = new List<TasksVM>();
                foreach (var task in tasks)
                {
                    var result = new TasksVM
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = task.Status.ToString(),
                        DueDate = task.DueDate.Date,
                        Priority = task.Priority.ToString()
                    };

                    taskVm.Add(result);
                }
                return Ok(taskVm);
            }
            catch (Exception)
            {
                throw new Exception("There is something wrong.");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int id)
        {
            var task = await _unitOfWork.Tasks.GetById(id);
            if (task == null)
            {
                return NotFound(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "No tasks found." });
            }
            var result = new TasksVM
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                DueDate = task.DueDate.Date,
                Priority = task.Priority.ToString()
            };
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddTask(Tasks task)
        {
            if (task == null)
            {
                return BadRequest(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "Invalid task details." });
            }

            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveAsync();

            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "Task added successfully." });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Tasks task)
        {
            var existingTask = await _unitOfWork.Tasks.GetFirstOrDefault(x => x.Id == id);
            if (existingTask == null)
            {
                return NotFound(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "No tasks found." });
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate.Date;
            existingTask.Priority = task.Priority;


            await _unitOfWork.Tasks.UpdateAsync(existingTask);
            await _unitOfWork.SaveAsync();

            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "Task updated successfully." });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var romoveTask = await _unitOfWork.Tasks.GetFirstOrDefault(x => x.Id == id);
            if (romoveTask == null)
            {
                return NotFound(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "No tasks found." });
            }

            await _unitOfWork.Tasks.Delete(romoveTask);
            await _unitOfWork.SaveAsync();

            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "Task deleted successfully." });
        }
    }
}




