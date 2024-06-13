using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EntityConfigurations;

internal class UserTypeConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("string")
            .IsRequired();

        builder.HasKey(x => x.Id);
    }
}