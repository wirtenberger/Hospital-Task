using AutoMapper;
using Hospital.Application.Doctors.Contracts;
using Hospital.Core.Model;

namespace Hospital.Application.Doctors;

public class DoctorMapper : Profile
{
	public DoctorMapper()
	{
		CreateMap<Doctor, EditDoctorDto>();

		CreateMap<Doctor, ListDoctorDto>()
			.ForPath(d => d.Speciality, e => e.MapFrom(s => s.Speciality.Name))
			.ForPath(d => d.Room, e => e.MapFrom(s => s.Room.Number))
			.ForPath(d => d.District, e => e.MapFrom(s => s.District == null ? new int?() : s.District.Number));
	}
}