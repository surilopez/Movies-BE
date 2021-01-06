using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies_BE.Entities;

namespace Movies_BE
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MoviesActors>()
                .HasKey(x => new { x.ActorID, x.MovieID });

            modelBuilder.Entity<MoviesGenres>()
                .HasKey(x => new { x.GenreID, x.MovieID });
            
            modelBuilder.Entity<MoviesTheaters>()
                .HasKey(x => new { x.TheaterID, x.MovieID });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Theaters> Theaters { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MoviesTheaters> MoviesTheaters { get; set; }


    }
}
