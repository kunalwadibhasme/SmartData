using Application.Model;
using Application.Model.Course;
using Application.Model.Enrollment;
using AutoMapper;
using Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<AddCourseDto, Course>();
            CreateMap<EnrollmentDto, Enrollments>();
            CreateMap<LoginDto, User>();
        }
    }
}
