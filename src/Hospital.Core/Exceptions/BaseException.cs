namespace Hospital.Core.Exceptions;

public class BaseException(int statusCode, string? detail = null) : Exception
{
	public int StatusCode { get; } = statusCode;

	public string? Detail { get; } = detail;
}