using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Application.Configuration;

public static class MvcBuilderExtensions
{
	public static IMvcBuilder AddJson(this IMvcBuilder mvcBuilder)
	{
		return mvcBuilder.AddJsonOptions(
			opts =>
			{
				opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				// Hidden because can show internal type names
				opts.AllowInputFormatterExceptionMessages = false;
			}
		);
	}

	public static IMvcBuilder ConfigureInvalidModelStateResponse(this IMvcBuilder mvcBuilder)
	{
		return mvcBuilder.ConfigureApiBehaviorOptions(
			opts =>
			{
				opts.InvalidModelStateResponseFactory = context =>
					new BadRequestObjectResult(new ApiErrorResponse(context.ModelState));
			}
		);
	}
}