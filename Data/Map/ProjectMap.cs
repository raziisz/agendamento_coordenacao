using agendamento_coordenacao.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agendamento_coordenacao.Data.Map
{
    public class ProjectMap : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("project");

            builder.Property(p => p.EndProject)
                .HasColumnName("end_project")
                .HasColumnType("datetime");
            
            builder.Property(p => p.StartProject)
                .HasColumnName("start_project")
                .HasColumnType("datetime");
        }
    }
}