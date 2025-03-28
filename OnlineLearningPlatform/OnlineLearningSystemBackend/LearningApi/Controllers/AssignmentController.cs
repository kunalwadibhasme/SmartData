using Application.App.Assignment.Command;
using Application.App.Assignment.Query;
using Application.App.Course.Command;
using Application.Model.Assignment;
using Application.Model.Course;
using Domain.Enitities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace LearningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AssignmentController> _logger;


        public AssignmentController(IMediator mediator, ILogger<AssignmentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("AddAssignment")]
        public async Task<IActionResult> AddAssignment(AssignmentDto assignmentDto)
        {
            var result = await _mediator.Send(new AddNewAssignmentCommand{ assignmentDto = assignmentDto });
            _logger.LogInformation("The function to Add NewAssignment is Called" + result);
            return Ok(result);
        }

        [HttpGet("GetAssignment")]
        public async Task<IActionResult> GetAssignment()
        {
            var result = await _mediator.Send(new GetAssignmentQuery { });
            _logger.LogInformation("This function to get Assignment is Called" + result);
            return Ok(result);
        }

        [HttpPost("createassignment")]
        public async Task<IActionResult> CreateAssignment(CreateAssignmentDto createAssignmentDto)
        {
            var result = await _mediator.Send(new CreateAssignmentCommand {createAssignmentDto = createAssignmentDto });
            return Ok(result);
        }

        [HttpGet("GetAssignmentById")]
        public async Task<IActionResult> GetAssignmentById(int Id)
        {
            var result = await _mediator.Send(new GetAssignmentTestQuery { AssignmentId = Id });
            _logger.LogInformation("This function to get Assignment by Id is Called" + result);
            return Ok(result);
        }
    }
}
