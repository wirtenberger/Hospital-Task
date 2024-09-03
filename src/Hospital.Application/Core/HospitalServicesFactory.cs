using System.Data;
using Hospital.Core.Abstractions;
using Hospital.DataAccess;
using Hospital.DataAccess.Abstractions;

namespace Hospital.Application.Core;

public class HospitalServicesFactory(
	HospitalUnitOfWorkFactory unitOfWorkFactory,
	IDateTimeProvider dateTimeProvider,
	IServiceProvider serviceProvider
)
{
	private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

	private readonly IServiceProvider _serviceProvider = serviceProvider;
	private readonly HospitalUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;

	public async Task WithTransaction(
		Func<HospitalServices, Task> op,
		IsolationLevel level,
		CancellationToken cancellationToken
	)
	{
		_ = await WithTransaction(
			async uow =>
			{
				await op(uow);
				return 1;
			},
			level,
			cancellationToken
		);
	}

	public Task<T> WithTransaction<T>(
		Func<HospitalServices, Task<T>> op,
		IsolationLevel level,
		CancellationToken cancellationToken
	)
	{
		return _unitOfWorkFactory.WithRetry(
			u =>
			{
				var services = CreateServices(u);
				return op(services);
			},
			level,
			cancellationToken
		);
	}

	private HospitalServices CreateServices(IHospitalUnitOfWork unitOfWork)
	{
		return new HospitalServices(
			new DoctorService(unitOfWork, _serviceProvider.GetRequiredService<ILogger<DoctorService>>()),
			new PatientService(
				unitOfWork,
				_dateTimeProvider,
				_serviceProvider.GetRequiredService<ILogger<PatientService>>()
			)
		);
	}
}