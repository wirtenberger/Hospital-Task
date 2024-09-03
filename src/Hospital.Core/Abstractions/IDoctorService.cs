using System.Linq.Expressions;
using Hospital.Core.Model;

namespace Hospital.Core.Abstractions;

public interface IDoctorService
{
	Task<Doctor> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	Task<Page<Doctor>> PaginatedAsync(
		int offset,
		int limit,
		Expression<Func<Doctor, bool>>? wherePredicate = null,
		Func<IQueryable<Doctor>, IQueryable<Doctor>>? orderBy = null
	);

	Task<Doctor> CreateAsync(
		string surname,
		string name,
		string? patronymic,
		int roomId,
		int specialityId,
		int? districtId,
		CancellationToken cancellationToken
	);

	Task<Doctor> EditAsync(Doctor editDoctor, CancellationToken cancellationToken);
	Task<Doctor> Delete(Guid id, CancellationToken cancellationToken);
}