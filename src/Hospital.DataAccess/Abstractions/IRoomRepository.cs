using Hospital.Core.Model;

namespace Hospital.DataAccess.Abstractions;

public interface IRoomRepository
{
	ValueTask<Room?> GetByIdAsync(int id, CancellationToken cancellationToken);
	Task<Room?> GetByNumber(int number, CancellationToken cancellationToken);
}