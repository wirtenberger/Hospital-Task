using Hospital.Core.Model;

namespace Hospital.DataAccess.Abstractions;

public interface ISpecialityRepository
{
	ValueTask<Speciality?> GetByIdAsync(int id, CancellationToken cancellationToken);
	Task<Speciality?> GetByName(string name, CancellationToken cancellationToken);
}