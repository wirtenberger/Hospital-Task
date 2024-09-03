namespace Hospital.Core.Abstractions;

public record Page<T>(
	List<T> Items,
	int TotalCount,
	int PagesCount
);