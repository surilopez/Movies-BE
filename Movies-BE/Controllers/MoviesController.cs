using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_BE.DTOs;
using Movies_BE.Entities;
using Movies_BE.Utilities;

namespace Movies_BE.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IStorageFile storageFile;
        private readonly string container = "movies";

        public MoviesController(ApplicationDBContext context, IMapper mapper, IStorageFile storageFile)
        {
            this.context = context;
            this.mapper = mapper;
            this.storageFile = storageFile;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await context.Movies
                .Include(x => x.moviesGenres).ThenInclude(x => x.genre)
                .Include(x => x.moviesActors).ThenInclude(x => x.actor)
                .Include(x => x.moviesTheaters).ThenInclude(x => x.theater)
                .FirstOrDefaultAsync(x => x.id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDTO = mapper.Map<MovieDTO>(movie);
            movieDTO.movieActorsDTO.OrderBy(x => x.Order).ToList();

            return movieDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieAddDTO movieAddDTO)
        {
            var movie = mapper.Map<Movies>(movieAddDTO);

            if (movieAddDTO.Img != null)
            {
                movie.Img = await storageFile.SaveFiles(container, movieAddDTO.Img);
            }

            WriteActorsByOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviesPostGetDTO>> PostGet()
        {
            var theaters = await context.Theaters.ToListAsync();
            var genres = await context.Genres.ToListAsync();

            var theatersDTO = mapper.Map<List<TheaterDTO>>(theaters);
            var genresDTO = mapper.Map<List<GenresDTO>>(genres);

            return new MoviesPostGetDTO() { theaters = theatersDTO, genres = genresDTO };
        }


        private void WriteActorsByOrder(Movies movie)
        {
            if (movie.moviesActors != null)
            {
                for (int i = 0; i < movie.moviesActors.Count; i++)
                {
                    movie.moviesActors[i].Order = i;
                }
            }
        }
    }
}
