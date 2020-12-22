using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies_BE.DTOs;
using Movies_BE.Entities;
using Movies_BE.Filters;
using Movies_BE.Utilities;

namespace Movies_BE.Controllers
{
    [Route("api/genres")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController : ControllerBase
    {
        
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public GenresController(ILogger<GenresController> logger, ApplicationDBContext context, IMapper mapper )
        {
            
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]        
        public async Task<ActionResult<List<GenresDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {

            var queryable= context.Genres.AsQueryable();
            await HttpContext.InsertPaginationParamsOnHeader(queryable);
            var genres = await queryable.OrderBy(x => x.Name).Pagin(paginationDTO).ToListAsync();
            
            return mapper.Map<List<GenresDTO>>(genres);

        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genres>> Get(int Id)
        {

            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenresAddDTO genreAddDTO)
        {
            var genre = mapper.Map<Genres>(genreAddDTO);
            context.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genres genre)
        {

            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
