using Hospital.Core.Abstractions;

namespace Hospital.Application.Core;

public class HospitalServices(IDoctorService doctorService, IPatientService patientService)
{
	public IDoctorService DoctorService { get; } = doctorService;

	public IPatientService PatientService { get; } = patientService;
}