using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies_BE.Repos;

namespace Movies_BE.Controllers
{
    public class GenresController
    {
        private readonly IRepos repos;

        public GenresController(IRepos repos)
        {
            this.repos = repos;
        }
    }
}
