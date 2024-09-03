using Hospital.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.DataAccess.EntityConfiguration;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
	public void Configure(EntityTypeBuilder<Doctor> builder)
	{
		builder.HasKey(b => b.Id);

		builder.Property(b => b.Surname).IsRequired().HasMaxLength(255);
		builder.Property(b => b.Name).IsRequired().HasMaxLength(255);
		builder.Property(b => b.Patronymic).HasMaxLength(255);

		builder.Property(b => b.RoomId).IsRequired();
		builder.Property(b => b.SpecialityId).IsRequired();

		builder.HasOne(b => b.Room)
		       .WithMany()
		       .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(b => b.Speciality)
		       .WithMany()
		       .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(b => b.District)
		       .WithMany()
		       .OnDelete(DeleteBehavior.SetNull);
	}
}