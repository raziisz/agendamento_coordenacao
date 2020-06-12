using agendamento_coordenacao.Data.Map;
using agendamento_coordenacao.Models;
using Microsoft.EntityFrameworkCore;

namespace agendamento_coordenacao.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Reunion> Reunions { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // modelBuilder.ApplyConfiguration(new ProjectMap());
            // modelBuilder.ApplyConfiguration(new ReunionMap());
            modelBuilder.ApplyConfiguration(new SchedulesMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            // modelBuilder.ApplyConfiguration(new WorkMap());
        }



        
    }
}