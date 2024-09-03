using System.Linq.Expressions;
using Hospital.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DataAccess;

public static class PaginatedExtensions
{
	public async static Task<Page<T>> Paginated<T>(
		this IQueryable<T> q,
		int offset,
		int limit,
		Expression<Func<T, bool>>? wherePredicate = null,
		Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
		CancellationToken cancellationToken = default
	)
	{
		if (wherePredicate is not null)
		{
			q = q.Where(wherePredicate);
		}

		q = orderBy is not null ? orderBy(q) : q.OrderBy(_ => "");


		var totalCount = q.Count();
		q = q.Skip(offset).Take(limit);
		var items = await q.ToListAsync(cancellationToken);
		return new Page<T>(
			items,
			totalCount,
			totalCount % limit == 0
				? totalCount / limit
				: totalCount / limit + 1
		);
	}
}