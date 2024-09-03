using FluentValidation;

namespace Hospital.Application.Doctors.Contracts;

public class EditDoctorDto
{
	public Guid Id { get; set; }

	public string Surname { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string? Patronymic { get; set; } = null!;

	public int RoomId { get; set; }

	public int SpecialityId { get; set; }

	public int? DistrictId { get; set; }
}

public class EditPatientDtoValidator : AbstractValidator<EditDoctorDto>
{
	public EditPatientDtoValidator()
	{
		RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage($"{nameof(EditDoctorDto.Id)} is required");

		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Surname).NotEmpty();

		RuleFor(x => x.RoomId).GreaterThan(0)
		                      .WithMessage($"{nameof(EditDoctorDto.RoomId)} is not specified or less than 1");

		RuleFor(x => x.SpecialityId).GreaterThan(0)
		                            .WithMessage(
			                            $"{nameof(EditDoctorDto.SpecialityId)} is not specified or less than 1"
		                            );
	}
}