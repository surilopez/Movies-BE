using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies_BE.DTOs;
using Movies_BE.Entities;

namespace Movies_BE.Utilities
{
    public class AutoMappersProfiles:Profile
    {
        public AutoMappersProfiles()
        {
            CreateMap<Genres, GenresDTO>().ReverseMap();
            CreateMap<GenresAddDTO, Genres>();
        }
    }
}
