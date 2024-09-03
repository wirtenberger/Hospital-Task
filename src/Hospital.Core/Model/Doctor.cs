namespace Hospital.Core.Model;

public class Doctor
{
	public Guid Id { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; }

	public int RoomId { get; set; }

	public virtual Room Room { get; set; } = null!;

	public int SpecialityId { get; set; }

	public virtual Speciality Speciality { get; set; } = null!;

	public int? DistrictId { get; set; }

	public virtual District? District { get; set; }

	public static Doctor Create(
		Guid id,
		string surname,
		string name,
		string? patronymic,
		int roomId,
		int specialityId,
		int? districtId,
		Room? room = null,
		Speciality? speciality = null,
		District? district = null
	)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(surname);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		ArgumentOutOfRangeException.ThrowIfNegative(roomId);
		ArgumentOutOfRangeException.ThrowIfNegative(specialityId);
		if (districtId.HasValue)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(districtId.Value);
		}

		return new Doctor()
		{
			Id = id,
			Surname = surname,
			Name = name,
			Patronymic = patronymic,
			RoomId = roomId,
			Room = room!,
			SpecialityId = specialityId,
			Speciality = speciality!,
			DistrictId = districtId,
			District = district
		};
	}
}