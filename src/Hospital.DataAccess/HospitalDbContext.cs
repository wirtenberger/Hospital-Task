using Hospital.Core.Model;
using Hospital.DataAccess.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Hospital.DataAccess;

public class HospitalDbContext(DbContextOptions<HospitalDbContext> options) : DbContext(options)
{
	public DbSet<District> Districts { get; protected set; } = null!;

	public DbSet<Room> Rooms { get; protected set; } = null!;

	public DbSet<Speciality> Specialities { get; protected set; } = null!;

	public DbSet<Patient> Patients { get; protected set; } = null!;

	public DbSet<Doctor> Doctors { get; protected set; } = null!;


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new DoctorConfiguration());
		modelBuilder.ApplyConfiguration(new PatientConfiguration());
		modelBuilder.ApplyConfiguration(new RoomConfiguration());
		modelBuilder.ApplyConfiguration(new DistrictConfiguration());
		modelBuilder.ApplyConfiguration(new SpecialityConfiguration());
	}
}