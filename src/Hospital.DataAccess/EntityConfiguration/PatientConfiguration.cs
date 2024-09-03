using Hospital.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.DataAccess.EntityConfiguration;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
	public void Configure(EntityTypeBuilder<Patient> builder)
	{
		builder.HasKey(b => b.Id);

		builder.Property(b => b.Surname).IsRequired().HasMaxLength(255);

		builder.Property(b => b.Name).IsRequired().HasMaxLength(255);

		builder.Property(b => b.Address).IsRequired().HasMaxLength(255);

		builder.Property(b => b.Patronymic).HasMaxLength(255);

		builder.Property(b => b.DateOfBirth).IsRequired();

		builder.Property(b => b.DistrictId).IsRequired();

		builder.Property(b => b.Sex).IsRequired()
		       .HasConversion<string>();

		builder.HasOne(b => b.District)
		       .WithMany()
		       .OnDelete(DeleteBehavior.Restrict);;
	}
}