using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public TasksController(IUnitOfWork unitOfWork, 
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
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


        [HttpPost]
        [Route("assigntask/{id}/{UserName}")]
        public async Task<IActionResult> AssignTask([FromRoute] int id, [FromRoute] string UserName)
        {
            var task = await _unitOfWork.Tasks.GetById(id);
            if (task == null)
            {
                return NotFound(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "Task not found." });
            }

            // Check if the user with the specified userId exists (you'll need to modify this)
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return NotFound(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "User not found." });
            }

            task.UserId = user.Id; // Assign the task to the specified user
            await _unitOfWork.SaveAsync();

            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "Task assigned successfully." });
        }


        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchTasks([FromQuery] string searchTerm)
        {
            // Fetch tasks that match the search term in their title or description
            var tasks = await _unitOfWork.Tasks.Search(searchTerm);

            var taskVm = tasks.Select(task => new TasksVM
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                DueDate = task.DueDate.Date,
                Priority = task.Priority.ToString()
            }).ToList();

            return Ok(taskVm);
        }

        [HttpGet]
        [Route("sort")]
        public async Task<IActionResult> SortTasks([FromQuery] string sortBy)
        {
            IEnumerable<Tasks> tasks;

            if (sortBy == "priority")
            {
                tasks = await _unitOfWork.Tasks.SortByPriority();
            }
            else if (sortBy == "dueDate")
            {
                tasks = await _unitOfWork.Tasks.SortByDueDate();
            }
            else
            {
                return BadRequest(new Response { StatusCode = StatusCodes.Status400BadRequest, Message = "Invalid sort parameter." });
            }

            var taskVm = tasks.Select(task => new TasksVM
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                DueDate = task.DueDate.Date,
                Priority = task.Priority.ToString()
            }).ToList();

            return Ok(taskVm);
        }

    }
}




