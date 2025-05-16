using EmocionesApi.Models;
using Microsoft.EntityFrameworkCore;
namespace EmocionesApi.Data
{
    public class AppDbcontext:DbContext
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options) : base(options) { }

        public DbSet<User> Usuarios { get; set; }

        public DbSet<DIarioEntrada> DiarioUser { get; set; }


    }
}
