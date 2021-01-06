using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies_BE.Utilities;

namespace Movies_BE.Controllers
{
    [ApiController]
    [Route("api/Movies")]
    public class MoviesController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper maper;
        private readonly IStorageFile storageFile;

        public MoviesController(ApplicationDBContext context, IMapper maper, IStorageFile storageFile)
        {
            this.context = context;
            this.maper = maper;
            this.storageFile = storageFile;
        }
    }
}
