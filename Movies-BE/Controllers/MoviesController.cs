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

        [HttpGet]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var today = DateTime.Today;

            var comingSoon = await context.Movies
                .Where(m => m.ReleaseDate > today)
                .OrderBy(m => m.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var onTheaters = await context.Movies
                .Where(m => m.onTheater)
                .OrderBy(m => m.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var result = new LandingPageDTO();
            result.commingSoom = mapper.Map<List<MovieDTO>>(comingSoon);
            result.onTheaters = mapper.Map<List<MovieDTO>>(onTheaters);

            return result;



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

        [HttpGet("PutGet")]
        public async Task<ActionResult<MoviePutGetDTO>> PutGet(int id)
        {
            var movieAR = await this.Get(id);
            if (movieAR.Result is NotFoundResult)
            {
                return NotFound("eeeeeeee");
            }

            var movie = movieAR.Value;

            var selectedGenresID = movie.genresDTO.Select(g => g.id).ToList();
            var noSelectedGenres = await context.Genres
                .Where(g => !selectedGenresID.Contains(g.id))
                .ToListAsync();

            var selectedTheaterID = movie.theaterDTO.Select(t => t.id).ToList();
            var noSelectedTheater = await context.Theaters
                .Where(t => !selectedTheaterID.Contains(t.id))
                .ToListAsync();

            var noselectedGenresDTO = mapper.Map<List<GenresDTO>>(noSelectedGenres);
            var noSelectedTheaterDTO = mapper.Map<List<TheaterDTO>>(noSelectedTheater);

            var result = new MoviePutGetDTO();
            result.movie = movie;
            result.selectedGenresDTO = movie.genresDTO;
            result.noselectedGenresDTO = noselectedGenresDTO;
            result.selectedTheaterDTO = movie.theaterDTO;
            result.noSelectedTheaterDTO = noSelectedTheaterDTO;
            result.movieActorsDTO = movie.movieActorsDTO;

            return result;

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieAddDTO movieAddDTO)
        {
            var movie = await context.Movies
                .Include(ma => ma.moviesActors)
                .Include(mg => mg.moviesGenres)
                .Include(mt => mt.moviesTheaters)
                .FirstOrDefaultAsync(x => x.id == id);

            if (movie == null)
            {
                return NotFound();
            }
            movie = mapper.Map(movieAddDTO, movie);

            if (movieAddDTO.Img != null)
            {
                movie.Img = await storageFile.EditFiles(container, movieAddDTO.Img, movie.Img);
            }

            WriteActorsByOrder(movie);
            await context.SaveChangesAsync();

            return NoContent();
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
