using Hospital.Core.Model;

namespace Hospital.DataAccess.Abstractions;

public interface IDoctorRepository
{
	ValueTask<Doctor?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	IQueryable<Doctor> GetAll();
	Task<Doctor> AddAsync(Doctor d, CancellationToken cancellationToken);
	void Update(Doctor d);
	void Remove(Doctor d);
}