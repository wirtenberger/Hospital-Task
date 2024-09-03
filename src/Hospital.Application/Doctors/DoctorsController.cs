using System.Data;
using AutoMapper;
using Hospital.Application.Core;
using Hospital.Application.Doctors.Contracts;
using Hospital.Core.Abstractions;
using Hospital.Core.Abstractions.Sorting;
using Hospital.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Application.Doctors;

[ApiController]
[Route("api/doctors")]
public class DoctorsController(
	HospitalServicesFactory hospitalServices,
	Sorter<Doctor> doctorSorter,
	IMapper mapper
) : ControllerBase
{
	private readonly Sorter<Doctor> _doctorSorter = doctorSorter;
	private readonly HospitalServicesFactory _hospitalServices = hospitalServices;
	private readonly IMapper _mapper = mapper;

	[HttpGet]
	public async Task<IActionResult> GetPaginated([FromQuery] PaginatedRequest request)
	{
		var paginated = await _hospitalServices.WithTransaction(
			async s =>
			{
				var res = await s.DoctorService.PaginatedAsync(
					request.Offset,
					request.Limit,
					orderBy: doctors => _doctorSorter.Ordered(
						doctors,
						request.SortBy,
						request.SortType
					)
				);

				return new Page<ListDoctorDto>(
					res.Items.Select(_mapper.Map<ListDoctorDto>).ToList(),
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
	public async Task<IActionResult> GetDoctor(Guid id)
	{
		var res = await _hospitalServices.WithTransaction(
			async s =>
			{
				var res = await s.DoctorService.GetByIdAsync(id, CancellationToken.None);
				return _mapper.Map<EditDoctorDto>(res);
			},
			IsolationLevel.ReadCommitted,
			CancellationToken.None
		);

		return Ok(res);
	}

	[HttpPut]
	public async Task<IActionResult> EditDoctor([FromBody] EditDoctorDto request)
	{
		await _hospitalServices.WithTransaction(
			async s =>
			{
				var doctor = Doctor.Create(
					request.Id,
					request.Surname,
					request.Name,
					request.Patronymic,
					request.RoomId,
					request.SpecialityId,
					request.DistrictId
				);

				await s.DoctorService.EditAsync(doctor, CancellationToken.None);
			},
			IsolationLevel.Serializable,
			CancellationToken.None
		);

		return Ok();
	}

	[HttpPost]
	public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request)
	{
		await _hospitalServices.WithTransaction(
			async s => await s.DoctorService.CreateAsync(
				request.Surname,
				request.Name,
				request.Patronymic,
				request.RoomId,
				request.SpecialityId,
				request.DistrictId,
				CancellationToken.None
			),
			IsolationLevel.Serializable,
			CancellationToken.None
		);

		return Ok(
		);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteDoctor(Guid id)
	{
		await _hospitalServices.WithTransaction(
			s => s.DoctorService.Delete(id, CancellationToken.None),
			IsolationLevel.Serializable,
			CancellationToken.None
		);

		return Ok();
	}
}