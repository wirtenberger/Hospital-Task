using System.Linq.Expressions;
using Hospital.Core.Abstractions;
using Hospital.Core.Exceptions;
using Hospital.Core.Model;
using Hospital.DataAccess;
using Hospital.DataAccess.Abstractions;

namespace Hospital.Application.Core;

public class PatientService(
	IHospitalUnitOfWork unitOfWork,
	IDateTimeProvider dateTimeProvider,
	ILogger<PatientService> logger
) : IPatientService
{
	private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
	private readonly ILogger<PatientService> _logger = logger;
	private readonly IHospitalUnitOfWork _unitOfWork = unitOfWork;

	public async Task<Patient> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var patient = await _unitOfWork.PatientRepository.GetById(id, cancellationToken);
		if (patient is null)
		{
			_logger.LogError("Couldn't find patient with id {id}", id);
			throw new PatientNotFoundException();
		}

		return patient;
	}

	public Task<Page<Patient>> Paginated(
		int offset,
		int limit,
		Expression<Func<Patient, bool>>? wherePredicate = null,
		Func<IQueryable<Patient>, IQueryable<Patient>>? orderBy = null,
		CancellationToken cancellationToken = default
	)
	{
		return _unitOfWork.PatientRepository.GetAll()
		                  .Paginated(offset, limit, wherePredicate, orderBy, cancellationToken);
	}

	public async Task<Patient> Edit(Patient editPatient, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Editing patient with id {id}", editPatient.Id);

		if (editPatient.DateOfBirth.ToDateTime(TimeOnly.MinValue) > _dateTimeProvider.UtcNow())
		{
			throw new BaseException(400, "Date of birth is from future");
		}

		if (!_unitOfWork.PatientRepository.GetAll().Any(d => d.Id == editPatient.Id))
		{
			_logger.LogError("Tried to edit non-existing patient");
			throw new PatientNotFoundException();
		}

		await CheckExists(editPatient.DistrictId, cancellationToken);
		_unitOfWork.PatientRepository.Update(editPatient);
		await _unitOfWork.SaveChangesAsync();
		return editPatient;
	}

	public async Task<Patient> Delete(Guid id, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Delete patient with id {id}", id);
		var patient = await GetByIdAsync(id, cancellationToken);
		_unitOfWork.PatientRepository.Remove(patient);
		await _unitOfWork.SaveChangesAsync();
		return patient;
	}

	public async Task<Patient> Create(
		string surname,
		string name,
		string? patronymic,
		string address,
		DateOnly dateOfBirth,
		Sex sex,
		int districtId,
		CancellationToken cancellationToken
	)
	{
		if (dateOfBirth.ToDateTime(TimeOnly.MinValue) > _dateTimeProvider.UtcNow())
		{
			throw new BaseException(400, "Date of birth is from future");
		}

		await CheckExists(districtId, cancellationToken);
		var patient = Patient.Create(Guid.NewGuid(), surname, name, patronymic, address, dateOfBirth, sex, districtId);
		patient = await _unitOfWork.PatientRepository.AddAsync(patient, cancellationToken);
		await _unitOfWork.SaveChangesAsync();
		return patient;
	}

	private async Task CheckExists(int districtId, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Check if district exists");
		var district = await _unitOfWork.DistrictRepository.GetByIdAsync(districtId, cancellationToken);
		if (district is null)
		{
			throw new DistrictNotFoundException();
		}
	}
}