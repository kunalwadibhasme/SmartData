using Application.App.Course.Command;
using Application.App.Course.Query;
using Application.Model.Course;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CourseController> _logger;

        public CourseController(IMediator mediator, ILogger<CourseController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse(AddCourseDto addCourseDto)
        {
            _logger.LogInformation("The function to Add NewCourse is Called");
            var course = await _mediator.Send( new CreateCourseCommand { AddCourse = addCourseDto } );
            return Ok(course);
        }

        [HttpGet("GetCourse")]
        public async Task<IActionResult> GetCourse(string Search)
        {
            _logger.LogInformation("The function to Add NewCourse is Called");
            var course = await _mediator.Send(new GetCourseQuery { search = Search });
            return Ok(course);
        }

        [HttpGet("GetAllCourse")]
        public async Task<IActionResult> GetCourses(int UserId)
        {
            _logger.LogInformation("The function to Get AllCourses is Called");
            var result = await _mediator.Send(new GetAllCourseQuery {UserId = UserId });
            return Ok(result);
        }

        [HttpGet("GetEnrolledCourse")]
        public async Task<IActionResult> GetEnrolledCourses(int UserId)
        {
            _logger.LogInformation("The function to Get EnrolledCourses is Called");
            var result = await _mediator.Send(new GetEnrolledCourses{ UserId = UserId });
            return Ok(result);
        }

        [HttpGet("GetInstructorCourses")]
        public async Task<IActionResult> GetInstructorCourses(int UserId)
        {
            _logger.LogInformation("The function to Get Courses Created by Instructor is Called");
            var result = await _mediator.Send(new GetCourseByInstructorIdQuery { Id = UserId });
            return Ok(result);
        }

        [HttpGet("GetCourseById")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            _logger.LogInformation("The function to Get Courses Created by Id is Called");
            var result = await _mediator.Send(new GetCourseByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse(UpdateCourseDto updateCourseDto)
        {
            _logger.LogInformation("The function to Update Course is Called");
            var result = await _mediator.Send(new UpdateCourseQuery { Updatecoursedto = updateCourseDto });
            return Ok(result);
        }

    }
}
