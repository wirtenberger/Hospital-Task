using System.Linq.Expressions;
using Hospital.Core.Abstractions;
using Hospital.Core.Exceptions;
using Hospital.Core.Model;
using Hospital.DataAccess;
using Hospital.DataAccess.Abstractions;

namespace Hospital.Application.Core;

public class DoctorService(IHospitalUnitOfWork unitOfWork, ILogger<DoctorService> logger) : IDoctorService
{
	private readonly ILogger<DoctorService> _logger = logger;
	private readonly IHospitalUnitOfWork _unitOfWork = unitOfWork;

	public async Task<Doctor> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id, cancellationToken);
		if (doctor is null)
		{
			throw new DoctorNotFoundException();
		}

		return doctor;
	}

	public Task<Page<Doctor>> PaginatedAsync(
		int offset,
		int limit,
		Expression<Func<Doctor, bool>>? wherePredicate = null,
		Func<IQueryable<Doctor>, IQueryable<Doctor>>? orderBy = null
	)
	{
		return _unitOfWork.DoctorRepository.GetAll()
		                  .Paginated(offset, limit, wherePredicate, orderBy);
	}


	public async Task<Doctor> CreateAsync(
		string surname,
		string name,
		string? patronymic,
		int roomId,
		int specialityId,
		int? districtId,
		CancellationToken cancellationToken
	)
	{
		await CheckExists(roomId, specialityId, districtId, cancellationToken);
		var doctor = Doctor.Create(Guid.NewGuid(), surname, name, patronymic, roomId, specialityId, districtId);
		doctor = await _unitOfWork.DoctorRepository.AddAsync(doctor, cancellationToken);
		await _unitOfWork.SaveChangesAsync();
		return doctor;
	}

	public async Task<Doctor> EditAsync(Doctor editDoctor, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Editing doctor with id {id}", editDoctor.Id);

		if (!_unitOfWork.DoctorRepository.GetAll().Any(d => d.Id == editDoctor.Id))
		{
			_logger.LogError("Tried to edit non-existing doctor");
			throw new DoctorNotFoundException();
		}

		await CheckExists(editDoctor.RoomId, editDoctor.SpecialityId, editDoctor.DistrictId, cancellationToken);

		_unitOfWork.DoctorRepository.Update(editDoctor);
		await _unitOfWork.SaveChangesAsync();
		return editDoctor;
	}

	public async Task<Doctor> Delete(Guid id, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Delete doctor with id {id}", id);
		var doctor = await GetByIdAsync(id, cancellationToken);
		_unitOfWork.DoctorRepository.Remove(doctor);
		await _unitOfWork.SaveChangesAsync();
		return doctor;
	}

	private async Task CheckExists(int roomId, int specialityId, int? districtId, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Check if room exists");
		var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId, cancellationToken);
		if (room is null)
		{
			_logger.LogError("New doctor's room not found");
			throw new RoomNotFoundException();
		}

		_logger.LogInformation("Check if speciality exists");
		var speciality = await _unitOfWork.SpecialityRepository.GetByIdAsync(specialityId, cancellationToken);
		if (speciality is null)
		{
			_logger.LogError("New doctor's speciality not found");
			throw new SpecialityNotFoundException();
		}

		if (districtId.HasValue)
		{
			_logger.LogInformation("Check if district exists");
			var district = await _unitOfWork.DistrictRepository.GetByIdAsync(districtId.Value, cancellationToken);
			if (district is null)
			{
				_logger.LogError("New doctor's district not found");
				throw new DistrictNotFoundException();
			}
		}
	}
}