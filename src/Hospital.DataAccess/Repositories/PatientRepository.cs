using Hospital.Core.Model;
using Hospital.DataAccess.Abstractions;

namespace Hospital.DataAccess.Repositories;

public class PatientRepository(HospitalDbContext dbContext) : IPatientRepository
{
	private readonly HospitalDbContext _dbContext = dbContext;


	public ValueTask<Patient?> GetById(Guid id, CancellationToken cancellationToken)
	{
		return _dbContext.Patients.FindAsync([id], cancellationToken);
	}

	public IQueryable<Patient> GetAll()
	{
		return _dbContext.Patients;
	}

	public async Task<Patient> AddAsync(Patient p, CancellationToken cancellationToken)
	{
		var entry = await _dbContext.Patients.AddAsync(p, cancellationToken);
		return entry.Entity;
	}

	public void Update(Patient p)
	{
		_dbContext.Patients.Update(p);
	}

	public void Remove(Patient p)
	{
		_dbContext.Patients.Remove(p);
	}
}