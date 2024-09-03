using Hospital.Core.Model;

namespace Hospital.DataAccess.Abstractions;

public interface IDistrictRepository
{
	ValueTask<District?> GetByIdAsync(int id, CancellationToken cancellationToken);
	Task<District?> GetByNumber(int number, CancellationToken cancellationToken);
}