using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies_BE.Entities;

namespace Movies_BE.Repos
{
  public interface IRepos
    {
        Task<Genres> GetGenreById(int Id);
        List<Genres> GetGenres();
    }
}
