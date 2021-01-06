using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies_BE.DTOs;
using Movies_BE.Entities;
using Movies_BE.Utilities;

namespace Movies_BE.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    public class MoviesController: ControllerBase
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

        [HttpPost]

        public async Task<ActionResult> Post([FromForm] MovieAddDTO movieAddDTO) {
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

        private void WriteActorsByOrder(Movies movie) {
            if (movie.moviesActors!=null)
            {
                for (int i = 0; i < movie.moviesActors.Count; i++)
                {
                    movie.moviesActors[i].Order = i;
                }
            }
        }
    }
}
