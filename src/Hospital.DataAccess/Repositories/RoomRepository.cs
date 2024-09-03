using Hospital.Core.Model;
using Hospital.DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DataAccess.Repositories;

public class RoomRepository(HospitalDbContext dbContext) : IRoomRepository
{
	private readonly HospitalDbContext _dbContext = dbContext;

	public ValueTask<Room?> GetByIdAsync(int id, CancellationToken cancellationToken)
	{
		return _dbContext.Rooms.FindAsync([id], cancellationToken);
	}

	public Task<Room?> GetByNumber(int number, CancellationToken cancellationToken)
	{
		return _dbContext.Rooms.SingleOrDefaultAsync(r => r.Number == number, cancellationToken);
	}
}