using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.DataAccess.Configuration;

public static class ServiceCollectionExtensinos
{
	public static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextFactory<HospitalDbContext>(
			dbOpts =>
			{
				dbOpts.UseSqlServer(
					configuration.GetConnectionString("SqlServer"),
					opts =>
					{
						opts.EnableRetryOnFailure(
							3,
							TimeSpan.FromSeconds(10),
							[]
						);
					}
				).UseLazyLoadingProxies(opts => opts.IgnoreNonVirtualNavigations());
			}
		);

		services.AddSingleton<HospitalUnitOfWorkFactory>();

		return services;
	}
}