using Hospital.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.DataAccess.EntityConfiguration;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
	public void Configure(EntityTypeBuilder<District> builder)
	{
		builder.ToTable(
			t =>
				t.HasCheckConstraint("Districts_Number_NonNegative", "Number > 0")
		);

		builder.HasKey(b => b.Id);
		builder.Property(b => b.Id).ValueGeneratedOnAdd();

		builder.Property(b => b.Number).IsRequired();
		builder.HasIndex(b => b.Number)
		       .IsUnique();

		builder.HasData(
			[
				District.Create(1, 1),
				District.Create(2, 2),
				District.Create(3, 3),
				District.Create(4, 4),
				District.Create(5, 5)
			]
		);
	}
}