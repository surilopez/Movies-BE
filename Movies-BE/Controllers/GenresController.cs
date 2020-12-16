using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Movies_BE.Entities;
using Movies_BE.Repos;

namespace Movies_BE.Controllers
{
    [Route("api/genres")]
    public class GenresController:ControllerBase
    {
        private readonly IRepos repos;

        public GenresController(IRepos repos)
        {
            this.repos = repos;
        }

        [HttpGet]
        [HttpGet("AllGenres")]
        [HttpGet("/AllGenres")]
        public ActionResult<List<Genres>> Get()
        {
            return repos.GetGenres();
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genres>> Get(int Id) {
            var genre = await repos.GetGenreById(Id);

            if (genre==null)
            {
               return NotFound();
            }
            return genre;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genres genre)
        {
            return NoContent();
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genres genre)
        {
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }
    }
}
