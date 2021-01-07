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

            CreateMap<Movies, MovieDTO>()
                .ForMember(x => x.genresDTO, opt => opt.MapFrom(MapMoviesGenresDTO))
                .ForMember(x => x.movieActorsDTO, opt => opt.MapFrom(MapMoviesActorsDTO))
                .ForMember(x => x.theaterDTO, opt => opt.MapFrom(MapMoviesTheaterDTO));

        }

        private List<TheaterDTO> MapMoviesTheaterDTO(Movies movie, MovieDTO movieDTO)
        {
            var result = new List<TheaterDTO>();
            if (movie.moviesActors != null)
            {
                foreach (var movieTheater in movie.moviesTheaters)
                {
                    result.Add(new TheaterDTO()
                    {
                        id = movieTheater.TheaterID,
                        Name = movieTheater.theater.Name,
                        Latitude = movieTheater.theater.Location.Y,
                        Longitude = movieTheater.theater.Location.X,
                        
                    });
                }
            }

            return result;
        }
        private List<MovieActorDTO> MapMoviesActorsDTO(Movies movie, MovieDTO movieDTO)
        {
            var result = new List<MovieActorDTO>();
            if (movie.moviesActors != null)
            {
                foreach (var moviesActor in movie.moviesActors)
                {
                    result.Add(new MovieActorDTO()
                    {
                        id = moviesActor.ActorID,
                        Name = moviesActor.actor.Name,
                        Photo = moviesActor.actor.Photo,
                        Order = moviesActor.Order,
                        Character = moviesActor.Character
                    });
                }
            }

            return result;
        }

        private List<GenresDTO> MapMoviesGenresDTO(Movies movie, MovieDTO movieDTO)
        {
            var result = new List<GenresDTO>();
            if (movie.moviesGenres != null)
            {
                foreach (var genre in movie.moviesGenres)
                {
                    result.Add(new GenresDTO() { id = genre.GenreID, Name = genre.genre.Name });
                }
            }

            return result;
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
