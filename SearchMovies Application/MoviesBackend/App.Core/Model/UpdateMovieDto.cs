using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class UpdateMovieDto
    {
        public required string MovieName { get; set; }
        public required string Year { get; set; }
        public IFormFile? Posterimage { get; set; }
    }
}
