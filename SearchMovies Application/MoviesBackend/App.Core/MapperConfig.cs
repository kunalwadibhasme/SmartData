using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using App.Core.Model;


namespace App.Core
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AddMovieDto, Movies>();
            CreateMap<UpdateMovieDto, Movies>();
            CreateMap<RegisterationDto, Users>();
        }

    }
}
