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
    public class AutoMappersProfiles : Profile
    {
        public AutoMappersProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genres, GenresDTO>().ReverseMap();
            CreateMap<GenresAddDTO, Genres>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorAddDTO, Actor>()
                .ForMember(x => x.Photo, options => options.Ignore());

            CreateMap<TheaterAddDTO, Theaters>()
                .ForMember(x => x.Location, x => x.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<Theaters, TheaterDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(field => field.Location.Y))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(field => field.Location.X));

            CreateMap<MovieAddDTO, Movies>()
                .ForMember(x => x.Img, opt => opt.Ignore())
                .ForMember(x => x.moviesGenres, opt => opt.MapFrom(MapMoviesGenres))
                .ForMember(x => x.moviesTheaters, opt => opt.MapFrom(MapMoviesTheaters))
                .ForMember(x => x.moviesActors, opt => opt.MapFrom(MapMoviesActors));

        }

        private List<MoviesActors> MapMoviesActors(MovieAddDTO movieAddDTO, Movies movie)
        {
            var result = new List<MoviesActors>();

            if (movieAddDTO.ActorsList == null)
            {
                return result;
            }
            foreach (var item in movieAddDTO.ActorsList)
            {
                result.Add(new MoviesActors() { ActorID = item.id, Character = item.Character });
            }

            return result;
        }

        private List<MoviesGenres> MapMoviesGenres(MovieAddDTO movieAddDTO, Movies movie)
        {

            var result = new List<MoviesGenres>();

            if (movieAddDTO.GenresIDList == null)
            {
                return result;
            }
            foreach (var item in movieAddDTO.GenresIDList)
            {
                result.Add(new MoviesGenres() { GenreID = item });
            }

            return result;
        }

        private List<MoviesTheaters> MapMoviesTheaters(MovieAddDTO movieAddDTO, Movies movie)
        {

            var result = new List<MoviesTheaters>();

            if (movieAddDTO.TheatersIDList == null)
            {
                return result;
            }
            foreach (var item in movieAddDTO.TheatersIDList)
            {
                result.Add(new MoviesTheaters() { TheaterID = item });
            }

            return result;
        }
    }
}
