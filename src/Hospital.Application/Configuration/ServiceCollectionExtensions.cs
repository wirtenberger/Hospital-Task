using Hospital.Application.Core;
using Hospital.Application.Doctors;
using Hospital.Application.Middlewares;
using Hospital.Application.Patients;
using Hospital.Core.Abstractions;
using Hospital.Core.Abstractions.Sorting;
using Hospital.Core.Model;
using Hospital.DataAccess.Configuration;

namespace Hospital.Application.Configuration;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddCore();

		services.AddUnitOfWork(configuration);
		services.AddSingleton<HospitalServicesFactory>();

		services.AddAutoMapper();

		return services;
	}

	private static IServiceCollection AddCore(this IServiceCollection services)
	{
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddSingleton(typeof(Sorter<>));
		services.AddSingleton<ISortProvider<Doctor>, DoctorSort>();
		services.AddSingleton<ISortProvider<Patient>, PatientSort>();
		services.AddSingleton<ExceptionHandlerMiddlware>();
		return services;
	}

	private static IServiceCollection AddAutoMapper(this IServiceCollection services)
	{
		return services.AddAutoMapper(
			opts =>
			{
				opts.CreateProfile(
					"default",
					expression =>
						expression.CreateMap<DateTime, DateOnly>().ReverseMap()
						          .ConvertUsing(e => e.ToDateTime(TimeOnly.MinValue))
				);

				opts.AddProfile<DoctorMapper>();
				opts.AddProfile<PatientMapper>();
			}
		);
	}
}