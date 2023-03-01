using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppNt.Models;



namespace RankingEquipos.Context
{
    public class RankingDataBaseContext : DbContext
    {
        public RankingDataBaseContext(DbContextOptions<RankingDataBaseContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Confederation> Confederations  { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<AppNt.Models.TeamVote> TeamVote { get; set; }






    }
}