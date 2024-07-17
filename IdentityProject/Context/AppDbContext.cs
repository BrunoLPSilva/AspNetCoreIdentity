using IdentityProject.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityProject.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding data
            modelBuilder.Entity<Aluno>().HasData(new Aluno
            {
                AlunoId = 1,
                Nome = "Bruno Silva",
                Email = "bSilva@hotmail.com",
                Idade = 28,
                Curso = "Matematica"
            });
        }
    }
}
