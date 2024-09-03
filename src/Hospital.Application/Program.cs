using FluentValidation;
using FluentValidation.AspNetCore;
using Hospital.Application.Configuration;
using Hospital.Application.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJson().ConfigureInvalidModelStateResponse();

ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddlware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();