using System;
using agendamento_coordenacao.Models;
using agendamento_coordenacao.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace agendamento_coordenacao.Data.Map
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id");

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .IsRequired();
            builder.Property(u => u.LastName)
                .HasColumnName("lastname")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(200)")
                .IsRequired();

            builder.Property(u => u.PasswordSalt)
                .HasColumnName("password_salt")
                .IsRequired();
            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();
            
            builder.Property(x => x.TypeUser)
                .HasColumnName("type_user")
                .HasMaxLength(50)
                .HasConversion(
                    e => e.ToString(),
                    e => (TypeUser)Enum.Parse(typeof(TypeUser), e)
                )
                .IsUnicode();
            
            builder.HasMany(u => u.Schedules).WithOne(s => s.User);
        }
    }
}