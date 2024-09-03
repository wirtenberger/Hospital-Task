using System.Linq.Expressions;
using Hospital.Core.Abstractions.Sorting;
using Hospital.Core.Model;

namespace Hospital.Application.Doctors;

public class DoctorSort : ISortProvider<Doctor>
{
	private readonly static Dictionary<string, Expression<Func<Doctor, object>>> Expressions = new()
	{
		{"id", doctor => doctor.Id},
		{"name", doctor => doctor.Name},
		{"surname", doctor => doctor.Surname}
		// ...
	};

	public Dictionary<string, Expression<Func<Doctor, object>>> Orders => Expressions;
}