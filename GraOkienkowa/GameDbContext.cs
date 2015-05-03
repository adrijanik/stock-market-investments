using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investments
{
    public class GameDbContext : DbContext
    {
        // Aby podac wlasna nazwe bazy danych, nalezy wywolac konstruktor bazowy z nazwą jako parametrem.
        public GameDbContext()
            : base("Inwestycje1")
        {
            // Użyj klasy GameDbInitializer do zainicjalizowania bazy danych.
            Database.SetInitializer<GameDbContext>(new GameDbInitializer());
        }

        public DbSet<Inwestycja> Inwestycja { get; set; }

        public DbSet<Grupa> Grupa { get; set; }

        public DbSet<Firma> Firma { get; set; }

        public DbSet<Operacja> Operacja { get; set; }

        public DbSet<Użytkownik> Użytkownik { get; set; }
    }

}
