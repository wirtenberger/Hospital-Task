using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hospital.Application.Configuration;

public class ApiErrorResponse
{
	private const string GeneralErrorKey = "requestError";

	public ApiErrorResponse(ModelStateDictionary contextModelState)
	{
		Errors = contextModelState
		         .Where(m => m.Value is not null && m.Value.ValidationState == ModelValidationState.Invalid)
		         .Select(Selector)
		         .ToDictionary();

		return;

		(string, IEnumerable<string>) Selector(KeyValuePair<string, ModelStateEntry?> m)
			=> (JsonNamingPolicy.CamelCase.ConvertName(m.Key), m.Value!.Errors.Select(e => e.ErrorMessage));
	}

	public ApiErrorResponse(string generalError)
	{
		Errors = new Dictionary<string, IEnumerable<string>>()
		{
			{GeneralErrorKey, [generalError]}
		};
	}

	public Dictionary<string, IEnumerable<string>> Errors { get; }
}