using System.Linq.Expressions;

namespace Hospital.Core.Abstractions.Sorting;

public interface ISortProvider<T>
{
	Dictionary<string, Expression<Func<T, object>>> Orders { get; }
}