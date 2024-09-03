using Hospital.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.DataAccess.EntityConfiguration;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
	public void Configure(EntityTypeBuilder<Room> builder)
	{
		builder.ToTable(
			t =>
				t.HasCheckConstraint("Rooms_Number_NonNegative", "Number > 0")
		);

		builder.HasKey(b => b.Id);
		builder.Property(b => b.Id).ValueGeneratedOnAdd();

		builder.Property(b => b.Number).IsRequired();
		builder.HasIndex(b => b.Number)
		       .IsUnique();

		builder.HasData(
			[
				Room.Create(1, 1),
				Room.Create(2, 2),
				Room.Create(3, 3),
				Room.Create(4, 4),
				Room.Create(5, 5)
			]
		);
	}
}