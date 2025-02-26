using App.Core.App.Movies.Command;
using App.Core.App.Movies.Querys;
using App.Core.App.User.Command;
using App.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMediator mediator, ILogger<MovieController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize("Admin")]
        [HttpPost("AddMovie")]
        public async Task<IActionResult> AddMovie([FromForm] AddMovieDto addMovieDto )
        {
            _logger.LogInformation("AddMovie function is called for adding new Movie");
            var userId = await _mediator.Send(new CreateMovieCommand { AddMovie = addMovieDto });
            return Ok(userId);
        }

        [Authorize("Admin")]
        [HttpPut("UpdateMovie")]
        public async Task<IActionResult> UpdateMovie(int Id , [FromForm] UpdateMovieDto update)
        {
            _logger.LogInformation("UpdateMovie function is called for updating Movie");
            var updatedata = await _mediator.Send(new UpdateMovieCommand { Id = Id, UpdateMovieDto = update});
            return Ok(updatedata);
        }

        [Authorize("Admin")]
        [HttpDelete("DeleteMovie")]
        public async Task<IActionResult> DeleteMovie(int movieid)
        {
            _logger.LogInformation("DeletingMovie function is called for Deleting movie");
            var result = await _mediator.Send(new DeleteMovieCommand { Id = movieid });
            return Ok(result);
        }

        [HttpGet("GetMovie")]
        public async Task<IActionResult> GetMovie()
        {
            _logger.LogInformation("GetMovie function is called for Fetching movie");
            var result = await _mediator.Send(new GetAllMovieQuery { });
            return Ok(result);
        }

        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> getAllMovies([FromQuery] string search, [FromQuery] string apikey)
        {
            // Validate the API key
            if (string.IsNullOrEmpty(apikey))
            {
                return Unauthorized(new { Message = "Invalid or missing API key" });
            }

            // Call the query with the search string
            var allMovie = await _mediator.Send(new GetMovieQuery { search = search });
            return Ok(allMovie);
        }
    }
}
