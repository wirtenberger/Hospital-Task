namespace Hospital.Core.Model;

public class Patient
{
	public Guid Id { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; }

	public string Address { get; set; } = null!;

	public DateOnly DateOfBirth { get; set; }

	public Sex Sex { get; set; }

	public int DistrictId { get; set; }

	public virtual District District { get; set; } = null!;

	public static Patient Create(
		Guid id,
		string surname,
		string name,
		string? patronymic,
		string address,
		DateOnly dateOfBirth,
		Sex sex,
		int districtId,
		District? district = null
	)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(surname);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		ArgumentException.ThrowIfNullOrWhiteSpace(address);

		ArgumentOutOfRangeException.ThrowIfNegative(districtId);

		return new Patient
		{
			Id = id,
			Surname = surname,
			Name = name,
			Patronymic = patronymic,
			Address = address,
			DateOfBirth = dateOfBirth,
			Sex = sex,
			DistrictId = districtId,
			District = district!
		};
	}
}