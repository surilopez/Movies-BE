using System.Collections.Generic;
using Movies_BE.Entities;

namespace Movies_BE.Repos
{
    public class RepoOnMemory
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
    }
}
