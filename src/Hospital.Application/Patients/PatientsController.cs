using System.Data;
using AutoMapper;
using Hospital.Application.Core;
using Hospital.Application.Patients.Contracts;
using Hospital.Core.Abstractions;
using Hospital.Core.Abstractions.Sorting;
using Hospital.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Application.Patients;

[ApiController]
[Route("api/patients")]
public class PatientsController(
	HospitalServicesFactory hospitalServices,
	Sorter<Patient> patientSorter,
	IMapper mapper
) : ControllerBase
{
	private readonly HospitalServicesFactory _hospitalServices = hospitalServices;
	private readonly IMapper _mapper = mapper;
	private readonly Sorter<Patient> _patientSorter = patientSorter;

	[HttpGet]
	public async Task<IActionResult> GetPaginated([FromQuery] PaginatedRequest request)
	{
		var paginated = await _hospitalServices.WithTransaction(
			async s =>
			{
				var res = await s.PatientService.Paginated(
					request.Offset,
					request.Limit,
					orderBy: patients => _patientSorter.Ordered(
						patients,
						request.SortBy,
						request.SortType
					)
				);

				return new Page<ListPatientDto>(
					res.Items.Select(_mapper.Map<ListPatientDto>).ToList(),
					res.TotalCount,
					res.PagesCount
				);
			},
			IsolationLevel.ReadCommitted,
			CancellationToken.None
		);

		return Ok(paginated);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetPatient(Guid id)
	{
		var res = await _hospitalServices.WithTransaction(
			async s =>
			{
				var res = await s.PatientService.GetByIdAsync(id, CancellationToken.None);
				return _mapper.Map<EditPatientDto>(res);
			},
			IsolationLevel.ReadCommitted,
			CancellationToken.None
		);

		return Ok(res);
	}

	[HttpPut]
	public async Task<IActionResult> EditPatient([FromBody] EditPatientDto request)
	{
		await _hospitalServices.WithTransaction(
			async s =>
			{
				var patient = Patient.Create(
					request.Id,
					request.Surname,
					request.Name,
					request.Patronymic,
					request.Address,
					DateOnly.FromDateTime(request.DateOfBirth),
					request.Sex,
					request.DistrictId
				);

				await s.PatientService.Edit(patient, CancellationToken.None);
			},
			IsolationLevel.Serializable,
			CancellationToken.None
		);

		return Ok();
	}

	[HttpPost]
	public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequest request)
	{
		var patient = await _hospitalServices.WithTransaction(
			async s =>
			{
				var p = await s.PatientService.Create(
					request.Surname,
					request.Name,
					request.Patronymic,
					request.Address,
					DateOnly.FromDateTime(request.DateOfBirth),
					request.Sex,
					request.DistrictId,
					CancellationToken.None
				);

				return _mapper.Map<EditPatientDto>(p);
			},
			IsolationLevel.Serializable,
			CancellationToken.None
		);

		return Ok(patient);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeletePatient(Guid id)
	{
		await _hospitalServices.WithTransaction(
			s => s.PatientService.Delete(id, CancellationToken.None),
			IsolationLevel.Serializable,
			CancellationToken.None
		);

		return Ok();
	}
}