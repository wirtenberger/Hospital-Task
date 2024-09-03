using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Hospital.Core.Abstractions.Sorting;

public partial class Sorter<T>(ISortProvider<T> sortProvider)
{
	private readonly ISortProvider<T> _sortProvider = sortProvider;

	public IQueryable<T> Ordered(IQueryable<T> toOrder, string? orderBy, SortType type = SortType.Asc)
	{
		if (orderBy is null
		    || !OrderRegex().IsMatch(orderBy))
		{
			return toOrder.OrderBy(_ => "");
		}

		Expression<Func<T, object>>? firstOrderFunc = null;
		var orders = orderBy.Replace(" ", "").Split(",");
		var i = 0;
		for (; i < orders.Length; i++)
		{
			if (_sortProvider.Orders.TryGetValue(orders[i], out firstOrderFunc))
			{
				break;
			}
		}

		if (firstOrderFunc is null)
		{
			return toOrder.OrderBy(_ => "");
		}

		var q = OrderBy(firstOrderFunc, toOrder, type);
		foreach (var order in orders[(i + 1)..])
		{
			if (_sortProvider.Orders.TryGetValue(order, out var orderFunc))
			{
				q = ThenBy(orderFunc, q, type);
			}
		}

		return q;
	}

	private static IOrderedQueryable<TSort> OrderBy<TSort>(
		Expression<Func<TSort, object>> expression,
		IQueryable<TSort> toOrder,
		SortType type
	)
	{
		if (type == SortType.Asc)
		{
			return toOrder.OrderBy(expression);
		}

		return toOrder.OrderByDescending(expression);
	}

	private static IOrderedQueryable<TSort> ThenBy<TSort>(
		Expression<Func<TSort, object>> expression,
		IOrderedQueryable<TSort> toOrder,
		SortType type
	)
	{
		if (type == SortType.Asc)
		{
			return toOrder.ThenBy(expression);
		}

		return toOrder.ThenByDescending(expression);
	}

	[GeneratedRegex("^[^,]+(,[^,]+)*$")]
	private static partial Regex OrderRegex();
}