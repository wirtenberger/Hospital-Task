namespace Hospital.Core.Model;

public class Speciality
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public static Speciality Create(int id, string speciality)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
		ArgumentException.ThrowIfNullOrWhiteSpace(speciality);
		return new Speciality()
		{
			Id = id,
			Name = speciality
		};
	}
}