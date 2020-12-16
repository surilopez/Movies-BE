using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies_BE.Entities;

namespace Movies_BE.Repos
{
    public class RepoOnMemory:IRepos
    {
        private List<Genres> _genres;
        public RepoOnMemory()
        {
            _genres = new List<Genres>()
            {
                new Genres(){id=1,Name = "Action"},
                new Genres(){id=2,Name = "Drama"},
            };
        }


        public List<Genres> GetGenres() {
            return _genres;
        }

        public async Task<Genres> GetGenreById(int Id) {

            await Task.Delay(1);
            return _genres.FirstOrDefault(x => x.id == Id);
        }
    }
}
