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
    [Route("api/Theaters")]
    public class TheaterController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public TheaterController(
            ApplicationDBContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TheaterDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {

            var queryable = context.Theaters.AsQueryable();
            await HttpContext.InsertPaginationParamsOnHeader(queryable);
            var theater = await queryable.OrderBy(x => x.Name).Pagin(paginationDTO).ToListAsync();

            return mapper.Map<List<TheaterDTO>>(theater);

        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<TheaterDTO>> Get(int Id)
        {
            var theater = await context.Theaters.FirstOrDefaultAsync(x => x.id == Id);

            if (theater == null)
            {
                return NotFound();
            }
            return mapper.Map<TheaterDTO>(theater);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TheaterAddDTO theaterAddDTO)
        {
            var theather = mapper.Map<Theaters>(theaterAddDTO);
            context.Add(theather);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] TheaterAddDTO theaterAdd)
        {
            var theater = await context.Theaters.FirstOrDefaultAsync(x => x.id == Id);

            if (theater == null)
            {
                return NotFound();
            }

            theater = mapper.Map(theaterAdd, theater);

            await context.SaveChangesAsync();

            return NoContent();

        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var theater = await context.Theaters.AnyAsync(x => x.id == id);
            if (!theater)
            {
                return NotFound();
            }
            context.Remove(new Theaters() { id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
