using Hospital.Core.Model;

namespace Hospital.DataAccess.Abstractions;

public interface IPatientRepository
{
	ValueTask<Patient?> GetById(Guid id, CancellationToken cancellationToken);
	IQueryable<Patient> GetAll();
	Task<Patient> AddAsync(Patient d, CancellationToken cancellationToken);
	void Update(Patient d);
	void Remove(Patient d);
}