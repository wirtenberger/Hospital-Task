using System.Linq.Expressions;
using Hospital.Core.Abstractions.Sorting;
using Hospital.Core.Model;

namespace Hospital.Application.Patients;

public class PatientSort : ISortProvider<Patient>
{
	private readonly static Dictionary<string, Expression<Func<Patient, object>>> Expressions = new()
	{
		{"id", patient => patient.Id},
		{"name", patient => patient.Name},
		{"surname", patient => patient.Surname}
		// ...
	};

	public Dictionary<string, Expression<Func<Patient, object>>> Orders => Expressions;
}