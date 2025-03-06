using Microsoft.EntityFrameworkCore;

namespace ApiJuegoPsp.Models;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public DbSet<Jugador> Jugadores { get; set; } = null!;
}