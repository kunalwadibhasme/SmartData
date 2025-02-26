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
    public class UpdateMovieCommand : IRequest<object>
    {
        public int Id { get; set; }

        public UpdateMovieDto UpdateMovieDto { get; set; }
    }
    public class UpdateMovieCommandhandler : IRequestHandler<UpdateMovieCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public UpdateMovieCommandhandler(IAppDbContext appDbContext, IMapper mapper )
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
     
        public async Task<object> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            var update = command.UpdateMovieDto;
            var existingmovie = await _appDbContext.Set<Domain.Entities.Movies>().FirstOrDefaultAsync(u => u.Id == command.Id);

            if (existingmovie == null)
            {
                var responses = new
                {
                    status = 404,
                    Message = "Movie Not Found"
                };
                return responses;
            }

            // Update the properties of the existing movie
            existingmovie.MovieName = update.MovieName;
            existingmovie.Year = update.Year;
            existingmovie.Deleted = false;

            if (update.Posterimage != null)
            {
                string imagepath = await UploadImagesAsync(update.Posterimage);
                existingmovie.Posterimage = imagepath;
            }

            _appDbContext.Set<Domain.Entities.Movies>().Update(existingmovie);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            var response = new
            {
                status = 200,
                message = "Movie Updated Successfully",
                data = existingmovie
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
