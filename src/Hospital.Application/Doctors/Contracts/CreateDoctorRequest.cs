using FluentValidation;

namespace Hospital.Application.Doctors.Contracts;

public class CreateDoctorRequest
{
	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; }

	public int RoomId { get; set; }

	public int SpecialityId { get; set; }

	public int? DistrictId { get; set; }
}

public class CreateDoctorRequestValidator : AbstractValidator<CreateDoctorRequest>
{
	public CreateDoctorRequestValidator()
	{
		RuleFor(x => x.RoomId).GreaterThan(0)
		                      .WithMessage($"{nameof(CreateDoctorRequest.RoomId)} is not specified or less than 1");

		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Surname).NotEmpty();

		RuleFor(x => x.SpecialityId).GreaterThan(0).WithMessage(
			$"{nameof(CreateDoctorRequest.SpecialityId)} is not specified or less than 1"
		);
	}
}