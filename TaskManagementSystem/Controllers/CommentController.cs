using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DataAccess.Repository.IRepository;
using TaskManagementSystem.Models.Authentication;
using TaskManagementSystem.Models.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        [Route("addComments/{taskId}")]
        public async Task<IActionResult> AddCommentToTask([FromRoute] int taskId, [FromBody] string commentText)
        {
            var task = await _unitOfWork.Tasks.GetById(taskId);
            if (task == null)
            {
                return NotFound(new Response { StatusCode = StatusCodes.Status404NotFound, Message = "Task not found." });
            }

            var comment = new Comments
            {
                TasksId = task.Id,
                Content = commentText
            };

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveAsync();

            return Ok(new Response { StatusCode = StatusCodes.Status200OK, Message = "Comment added successfully." });
        }
    }
}
