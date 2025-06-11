
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoDIARS.Models;

namespace ProyectoDIARS.config;

public class CursoConfig : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");

        builder.HasKey(a => a.IdCurso);

        builder.Property(a => a.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.horario)
            .IsRequired();

        builder.Property(a => a.aula)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(a => a.DocenteId)
            .IsRequired();
    }
}
