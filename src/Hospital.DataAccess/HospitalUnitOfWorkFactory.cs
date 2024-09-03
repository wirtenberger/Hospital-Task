using System.Data;
using Hospital.DataAccess.Abstractions;
using Hospital.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DataAccess;

public class HospitalUnitOfWorkFactory(IDbContextFactory<HospitalDbContext> dbContextFactory)
{
	private readonly IDbContextFactory<HospitalDbContext> _dbContextFactory = dbContextFactory;

	public async Task WithRetry(
		Func<IHospitalUnitOfWork, Task> op,
		IsolationLevel level,
		CancellationToken cancellationToken
	)
	{
		_ = await WithRetry(
			async uow =>
			{
				await op(uow);
				return 1;
			},
			level,
			cancellationToken
		);
	}

	public async Task<T> WithRetry<T>(
		Func<IHospitalUnitOfWork, Task<T>> op,
		IsolationLevel level,
		CancellationToken cancellationToken
	)
	{
		var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		var unitOfWork = CreateUnitOfWork(dbContext);
		var executionStrategy = dbContext.Database.CreateExecutionStrategy();

		return await executionStrategy.ExecuteAsync(
			async c =>
			{
				await using var tx = await dbContext.Database.BeginTransactionAsync(level, c);
				try
				{
					var res = await op(unitOfWork);
					await tx.CommitAsync(c);
					return res;
				}
				catch
				{
					await tx.RollbackAsync(c);
					throw;
				}
			}, cancellationToken
		);
	}

	private static HospitalUnitOfWork CreateUnitOfWork(HospitalDbContext dbContext)
	{
		return new HospitalUnitOfWork(
			dbContext,
			new DistrictRepository(dbContext),
			new RoomRepository(dbContext),
			new SpecialityRepository(dbContext),
			new DoctorRepository(dbContext),
			new PatientRepository(dbContext)
		);
	}
}