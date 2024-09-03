using Hospital.Core.Abstractions.Sorting;

namespace Hospital.Core.Abstractions;

public record PaginatedRequest(
	int Offset = 0,
	int Limit = 50,
	SortType SortType = SortType.Asc,
	string? SortBy = null
);