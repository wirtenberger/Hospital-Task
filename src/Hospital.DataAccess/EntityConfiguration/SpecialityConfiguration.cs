using Hospital.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.DataAccess.EntityConfiguration;

public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
{
	public void Configure(EntityTypeBuilder<Speciality> builder)
	{
		builder.HasKey(b => b.Id);
		builder.Property(b => b.Id).ValueGeneratedOnAdd();

		builder.Property(b => b.Name).IsRequired().HasMaxLength(255);
		builder.HasIndex(b => b.Name)
		       .IsUnique();

		builder.HasData(
			[
				Speciality.Create(1, "Speciality 1"),
				Speciality.Create(2, "Speciality 2"),
				Speciality.Create(3, "Speciality 3"),
				Speciality.Create(4, "Speciality 4"),
				Speciality.Create(5, "Speciality 5")
			]
		);
	}
}