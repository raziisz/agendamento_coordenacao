using agendamento_coordenacao.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agendamento_coordenacao.Data.Map
{
    public class ReunionMap : IEntityTypeConfiguration<Reunion>
    {
        public void Configure(EntityTypeBuilder<Reunion> builder)
        {
            builder.ToTable("reunion");

            builder.Property(r => r.DateReunion)
                .HasColumnName("date_reunion")
                .HasColumnType("datetime");
            
            builder.Property(r => r.HourStart)
                .HasColumnName("hour_start")
                .HasColumnType("varchar(100)");

            builder.Property(r => r.HourEnd)
                .HasColumnName("end_hour")
                .HasColumnType("varchar(100)");
        }
    }
}