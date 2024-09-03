using Hospital.Core.Model;
using Hospital.DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DataAccess.Repositories;

public class SpecialityRepository(HospitalDbContext dbContext) : ISpecialityRepository
{
	private readonly HospitalDbContext _dbContext = dbContext;

	public ValueTask<Speciality?> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return _dbContext.Specialities.FindAsync([id], cancellationToken);
	}

	public Task<Speciality?> GetByName(string name, CancellationToken cancellationToken)
	{
		return _dbContext.Specialities.SingleOrDefaultAsync(s => s.Name == name, cancellationToken);
	}
}