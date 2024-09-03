using Hospital.DataAccess.Abstractions;

namespace Hospital.DataAccess;

public class HospitalUnitOfWork(
	HospitalDbContext dbContext,
	IDistrictRepository districtRepository,
	IRoomRepository roomRepository,
	ISpecialityRepository specialityRepository,
	IDoctorRepository doctorRepository,
	IPatientRepository patientRepository
) : IHospitalUnitOfWork
{
	private readonly HospitalDbContext _dbContext = dbContext;

	public IDistrictRepository DistrictRepository { get; } = districtRepository;

	public IRoomRepository RoomRepository { get; } = roomRepository;

	public ISpecialityRepository SpecialityRepository { get; } = specialityRepository;

	public IDoctorRepository DoctorRepository { get; } = doctorRepository;

	public IPatientRepository PatientRepository { get; } = patientRepository;

	public int SaveChanges()
	{
		return _dbContext.SaveChanges();
	}

	public Task<int> SaveChangesAsync()
	{
		return _dbContext.SaveChangesAsync();
	}
}