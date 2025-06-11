using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoDIARS.Models;

namespace ProyectoDIARS.config;

public class NotificacionConfig : IEntityTypeConfiguration<Notificacion>
{
    public void Configure(EntityTypeBuilder<Notificacion> builder)
    {
        builder.ToTable("Notificacion");

        builder.HasKey(a => a.IdNotificacion);



        builder.Property(a => a.TutorId)
            .IsRequired();

        builder.Property(a => a.fecha)
            .IsRequired();

        builder.Property(a => a.Titulo)
            .IsRequired();

        builder.Property(a => a.Mensaje)
           .IsRequired()
           .HasMaxLength(500);

        builder.Property(a => a.Leida)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.Tipo)
            .IsRequired()
            .HasMaxLength(50);
    }
}