using Hospital.Core.Model;
using Hospital.DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DataAccess.Repositories;

public class DistrictRepository(HospitalDbContext dbContext) : IDistrictRepository
{
	private readonly HospitalDbContext _dbContext = dbContext;

	public ValueTask<District?> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return _dbContext.Districts.FindAsync([id], cancellationToken);
	}

	public Task<District?> GetByNumber(int number, CancellationToken cancellationToken)
	{
		return _dbContext.Districts.SingleOrDefaultAsync(d => d.Number == number, cancellationToken);
	}
}