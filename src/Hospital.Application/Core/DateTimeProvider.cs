using Hospital.Core.Abstractions;

namespace Hospital.Application.Core;

public class DateTimeProvider : IDateTimeProvider
{
	public DateTime UtcNow()
	{
		return DateTime.UtcNow;
	}
}