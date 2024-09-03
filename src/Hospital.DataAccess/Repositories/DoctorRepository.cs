using Hospital.Core.Model;
using Hospital.DataAccess.Abstractions;

namespace Hospital.DataAccess.Repositories;

public class DoctorRepository(HospitalDbContext dbContext) : IDoctorRepository
{
	private readonly HospitalDbContext _dbContext = dbContext;

	public ValueTask<Doctor?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		return _dbContext.Doctors.FindAsync([id], cancellationToken);
	}

	public IQueryable<Doctor> GetAll()
	{
		return _dbContext.Doctors;
	}

	public async Task<Doctor> AddAsync(Doctor d, CancellationToken cancellationToken)
	{
		var entry = await _dbContext.Doctors.AddAsync(d, cancellationToken);
		return entry.Entity;
	}

	public void Update(Doctor d)
	{
		_dbContext.Doctors.Update(d);
	}

	public void Remove(Doctor d)
	{
		_dbContext.Doctors.Remove(d);
	}
}