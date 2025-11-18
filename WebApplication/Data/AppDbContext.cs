using HarmonyWork.Models;
using Microsoft.EntityFrameworkCore;

namespace HarmonyWork.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Aqui definimos quais tabelas serão criadas
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}