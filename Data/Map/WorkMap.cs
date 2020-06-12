using agendamento_coordenacao.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace agendamento_coordenacao.Data.Map
{
    public class WorkMap : IEntityTypeConfiguration<Work>
    {
        public void Configure(EntityTypeBuilder<Work> builder)
        {
            builder.ToTable("work");

            builder.Property(w => w.DateWork)
                .HasColumnName("date_work")
                .HasColumnType("datetime");
        }
    }
}