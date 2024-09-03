namespace Hospital.Application.Doctors.Contracts;

public class ListDoctorDto
{
	public Guid Id { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; } = null!;

	public int Room { get; set; }

	public string Speciality { get; set; } = null!;

	public int? District { get; set; }
}