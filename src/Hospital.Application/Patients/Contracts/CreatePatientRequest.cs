using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Hospital.Core.Model;

namespace Hospital.Application.Patients.Contracts;

public class CreatePatientRequest
{
	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; }

	public string Address { get; set; } = null!;

	public Sex Sex { get; set; }

	[DataType(DataType.Date)]
	public DateTime DateOfBirth { get; set; }

	public int DistrictId { get; set; }
}

public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
{
	public CreatePatientRequestValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Surname).NotEmpty();
		RuleFor(x => x.Address).NotEmpty();

		RuleFor(x => x.DateOfBirth).NotEqual(DateTime.MinValue)
		                           .WithMessage($"{nameof(CreatePatientRequest.DateOfBirth)} is required");

		RuleFor(x => x.DistrictId).GreaterThan(0)
		                          .WithMessage(
			                          $"{nameof(CreatePatientRequest.DistrictId)} is not specified or less than 1"
		                          );
	}
}