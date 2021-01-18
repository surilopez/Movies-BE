using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_BE.DTOs;
using Movies_BE.Entities;
using Movies_BE.Utilities;

namespace Movies_BE.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IStorageFile storageFile;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string container = "movies";

        public MoviesController(ApplicationDBContext context, IMapper mapper, IStorageFile storageFile,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.storageFile = storageFile;
            this.userManager = userManager;
        }

        //------------------------Endpoints POST-----------------------------------------
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] MovieAddDTO movieAddDTO)
        {
            var movie = mapper.Map<Movies>(movieAddDTO);

            if (movieAddDTO.Img != null)
            {
                movie.Img = await storageFile.SaveFiles(container, movieAddDTO.Img);
            }

            WriteActorsByOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.id;

        }
        //------------------------Endpoints GET-----------------------------------------
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
        [AllowAnonymous]
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
            var averageVote = 0.0;
            var userVote = 0;

            if (await context.Ratings.AnyAsync(x => x.movieId == id))
            {
                averageVote = await context.Ratings.Where(x => x.movieId == id)
                    .AverageAsync(x=>x.score);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    var user = await userManager.FindByEmailAsync(email);
                    var userId = user.Id;
                    var ratingDB = await context.Ratings
                        .FirstOrDefaultAsync(x => x.userId == userId && x.movieId == id);

                    if (ratingDB!=null)
                    {
                        userVote = ratingDB.score;
                    }
                }

            }

            var movieDTO = mapper.Map<MovieDTO>(movie);
            movieDTO.averageVote = averageVote;
            movieDTO.userVote = userVote;
            movieDTO.movieActorsDTO.OrderBy(x => x.Order).ToList();

            return movieDTO;
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

        [HttpGet("Filters")]

        public async Task<ActionResult<List<MovieDTO>>> Filters([FromQuery] MoviesFiltersDTO moviesFiltersDTO)
        {
            var queriablesMovies = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(moviesFiltersDTO.Title))
            {
                queriablesMovies = queriablesMovies.Where(x => x.Title.Contains(moviesFiltersDTO.Title));
            }

            if (moviesFiltersDTO.onTheaters)
            {
                queriablesMovies = queriablesMovies.Where(x => x.onTheater);
            }

            if (moviesFiltersDTO.commingSoon)
            {
                queriablesMovies = queriablesMovies.Where(x => x.ReleaseDate > DateTime.Today);
            }

            if (moviesFiltersDTO.genreID != 0)
            {
                queriablesMovies = queriablesMovies
                    .Where(x => x.moviesGenres.Select(g => g.GenreID).Contains(moviesFiltersDTO.genreID));
            }

            await HttpContext.InsertPaginationParamsOnHeader(queriablesMovies);

            var movies = await queriablesMovies.Pagin(moviesFiltersDTO.paginationDTO).ToListAsync();

            return mapper.Map<List<MovieDTO>>(movies);

        }


        //------------------------Endpoints PUT-----------------------------------------
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

        //------------------------Endpoints DELETE-----------------------------------------

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Movies.FirstOrDefaultAsync(x => x.id == id);
            if (exist == null)
            {
                return NotFound();
            }
            context.Remove(exist);
            await context.SaveChangesAsync();

            await storageFile.DeleteFiles(exist.Img, container);
            return NoContent();
        }


        //------------------------OTHERS FUNCTIONS-----------------------------------------
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
