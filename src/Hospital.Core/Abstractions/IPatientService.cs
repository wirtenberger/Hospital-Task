using System.Linq.Expressions;
using Hospital.Core.Model;

namespace Hospital.Core.Abstractions;

public interface IPatientService
{
	Task<Patient> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	Task<Page<Patient>> Paginated(
		int offset,
		int limit,
		Expression<Func<Patient, bool>>? wherePredicate = null,
		Func<IQueryable<Patient>, IQueryable<Patient>>? orderBy = null,
		CancellationToken cancellationToken = default
	);

	Task<Patient> Edit(Patient editPatient, CancellationToken cancellationToken);

	Task<Patient> Delete(Guid id, CancellationToken cancellationToken);

	Task<Patient> Create(
		string surname,
		string name,
		string? patronymic,
		string address,
		DateOnly dateOfBirth,
		Sex sex,
		int districtId,
		CancellationToken cancellationToken
	);
}