namespace Hospital.Core.Abstractions;

public interface IDateTimeProvider
{
	DateTime UtcNow();
}