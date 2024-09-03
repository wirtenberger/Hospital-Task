using AutoMapper;
using Hospital.Application.Patients.Contracts;
using Hospital.Core.Model;

namespace Hospital.Application.Patients;

public class PatientMapper : Profile
{
	public PatientMapper()
	{
		CreateMap<Patient, EditPatientDto>();

		CreateMap<Patient, ListPatientDto>()
			.ForPath(p => p.District, e => e.MapFrom(p => p.District.Number));
	}
}