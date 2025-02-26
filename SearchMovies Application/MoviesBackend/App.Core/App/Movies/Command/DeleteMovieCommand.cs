using App.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Movies.Command
{
    public class DeleteMovieCommand : IRequest<object>
    {
        public int Id { get; set; }
    }
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteMovieCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(DeleteMovieCommand command, CancellationToken cancellationToken)
        {
            var IfExist = await _appDbContext.Set<Domain.Entities.Movies>().FirstOrDefaultAsync(u =>u.Id == command.Id && u.Deleted == false);
            if (IfExist == null)
            {
                return new
                {
                    status = 404,
                    message = "Movie not present or Already Deleted"

                };
            }

            IfExist.Deleted = true;
            _appDbContext.Set<Domain.Entities.Movies>().Update(IfExist);
            await _appDbContext.SaveChangesAsync();
            return new
            {
                status = 200,
                message = "Movie Deleted Successfully"
            };
        }
    }
}
