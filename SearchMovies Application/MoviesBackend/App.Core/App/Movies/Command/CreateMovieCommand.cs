using App.Core.Interface;
using App.Core.Model;
using AutoMapper;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Movies.Command
{
    public class CreateMovieCommand : IRequest<object>
    {
        public required AddMovieDto AddMovie { get; set; }
    }
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CreateMovieCommandHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;

        }

        public async Task<object> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = command.AddMovie;
            var existingmovie = await _appDbContext.Set<Domain.Entities.Movies>().FirstOrDefaultAsync(u => u.MovieName == movie.MovieName);

            if (existingmovie != null)
            {
                var responses = new
                {
                    status = 409,
                    Message = "Movie Already Exists"
                };
                return responses;
            }

            string imagepath = await UploadImagesAsync(movie.Posterimage);
            var newmovie = _mapper.Map<Domain.Entities.Movies>(movie);
            //var newmovie = movie.Adapt<Domain.Entities.Movies>();
            newmovie.Posterimage = imagepath;
            newmovie.Deleted = false;
            await _appDbContext.Set<Domain.Entities.Movies>().AddAsync(newmovie);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            var response = new
            {
                status = 200,
                message = "Movies Added Successfully",
                data = newmovie
            };
            return response;

        }

        private async Task<string?> UploadImagesAsync(IFormFile profileimage)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filename = Guid.NewGuid().ToString() + "_" + profileimage.FileName;
            string filePath = Path.Combine(uploadsFolder, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profileimage.CopyTo(stream);
            }
            return $"/uploads/{filename}";
        }
    }


}
