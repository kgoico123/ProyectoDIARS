using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoDIARS.Models;

namespace ProyectoDIARS.config;

public class DocenteConfig : IEntityTypeConfiguration<Docente>
{
    public void Configure(EntityTypeBuilder<Docente> builder)
    {
        builder.ToTable("Docentes");

        builder.HasKey(a => a.IdDocente);

        builder.HasOne<ApplicationUser>()
            .WithMany() 
            .HasForeignKey(t => t.userId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.userId)
            .IsRequired();
    }
}