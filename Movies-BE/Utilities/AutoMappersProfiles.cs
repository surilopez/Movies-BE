using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies_BE.DTOs;
using Movies_BE.Entities;
using NetTopologySuite.Geometries;

namespace Movies_BE.Utilities
{
    public class AutoMappersProfiles:Profile
    {
        public AutoMappersProfiles(GeometryFactory geometryFactory )
        {
            CreateMap<Genres, GenresDTO>().ReverseMap();
            CreateMap<GenresAddDTO, Genres>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorAddDTO, Actor>()
                .ForMember(x => x.Photo, options => options.Ignore());

            CreateMap<TheaterAddDTO, Theaters>()
                .ForMember(x => x.Location, x => x.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Longitude))));

            CreateMap<Theaters, TheaterDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(field => field.Location.Y))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(field => field.Location.X));
        }
    }
}
