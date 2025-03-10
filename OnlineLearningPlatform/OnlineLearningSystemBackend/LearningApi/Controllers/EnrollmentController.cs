using Application.App.Course.Command;
using Application.App.Enrollment.Command;
using Application.Model.Course;
using Application.Model.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IMediator mediator, ILogger<EnrollmentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("Enroll")]
        public async Task<IActionResult> AddEnrollment(EnrollmentDto enrollmentDto)
        {
            _logger.LogInformation("The function to Add NewEnrollment is Called");
            var enroll = await _mediator.Send(new AddEnrollmentCommand { Enrollment = enrollmentDto });
            return Ok(enroll);
        }
    }
}
