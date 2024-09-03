using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Hospital.Core.Model;

namespace Hospital.Application.Patients.Contracts;

public class EditPatientDto
{
	public Guid Id { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; }

	public string Address { get; set; } = null!;

	[DataType(DataType.Date)]
	public DateTime DateOfBirth { get; set; }

	public Sex Sex { get; set; }

	public int DistrictId { get; set; }
}

public class EditPatientDtoValidator : AbstractValidator<EditPatientDto>
{
	public EditPatientDtoValidator()
	{
		RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage($"{nameof(EditPatientDto.Id)} is required");

		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Surname).NotEmpty();
		RuleFor(x => x.Address).NotEmpty();

		RuleFor(x => x.DateOfBirth).NotEqual(DateTime.MinValue)
		                           .WithMessage($"{nameof(EditPatientDto.DateOfBirth)} is required");

		RuleFor(x => x.DistrictId).GreaterThan(0)
		                          .WithMessage($"{nameof(EditPatientDto.DistrictId)} is not specified or less than 1");
	}
}