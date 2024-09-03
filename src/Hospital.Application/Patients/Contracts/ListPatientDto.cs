using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Patients.Contracts;

public class ListPatientDto
{
	public Guid Id { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; }

	public string Address { get; set; } = null!;

	[DataType(DataType.Date)]
	public DateTime DateOfBirth { get; set; }

	public int District { get; set; }
}