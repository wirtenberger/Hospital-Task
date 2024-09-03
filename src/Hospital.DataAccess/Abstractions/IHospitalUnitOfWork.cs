namespace Hospital.DataAccess.Abstractions;

public interface IHospitalUnitOfWork
{
	IDistrictRepository DistrictRepository { get; }

	IRoomRepository RoomRepository { get; }

	ISpecialityRepository SpecialityRepository { get; }

	IDoctorRepository DoctorRepository { get; }

	IPatientRepository PatientRepository { get; }

	int SaveChanges();

	Task<int> SaveChangesAsync();
}