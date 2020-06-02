using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Map
{
    public class SchedulesMap : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("schedules");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id");
            
            builder.Property(s => s.Title)
                .HasColumnName("title")
                .HasColumnType("varchar(250)")
                .IsRequired();
            
            builder.Property(s => s.Description)
                .HasColumnName("description")
                .HasColumnType("varchar(1024)")
                .IsRequired();
            
            builder.Property(s => s.Local)
                .HasColumnName("local")
                .HasColumnType("varchar(2028)")
                .IsRequired();
            
            builder.Property(s => s.Reschedule)
                .HasColumnName("reschedule")
                .HasColumnType("tinyint")
                .HasDefaultValue(0);
            
            builder.HasOne(u => u.User).WithMany(s => s.Schedules);
                
        }
    }
}