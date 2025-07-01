﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoDIARS.Models;

namespace ProyectoDIARS.config;

public class DocenteConfig : IEntityTypeConfiguration<Docente>
{
    public void Configure(EntityTypeBuilder<Docente> builder)
    {
        builder.ToTable("Docentes");

        builder.HasKey(a => a.IdDocente);

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.UserId)
           .IsRequired(false);
    }
}