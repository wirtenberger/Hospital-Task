using Hospital.Application.Core;
using Hospital.Core.Exceptions;
using Hospital.Core.Model;
using Hospital.DataAccess.Abstractions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Hospital.Application.Tests.Core;

[TestClass]
public class DoctorServiceTests
{
	private DoctorService _doctorService = null!;

	private readonly Mock<IHospitalUnitOfWork> _uowMock = new();

	[TestInitialize]
	public void Initialize()
	{
		_doctorService = new DoctorService(
			_uowMock.Object,
			NullLogger<DoctorService>.Instance
		);
	}

	#region GetByIdAsync

	[TestMethod]
	public async Task GetByIdAsync_DoctorNotFoundException_ThrowsDoctorNotFoundException()
	{
		_uowMock.Setup(
			u => u.DoctorRepository.GetByIdAsync(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
			() => _doctorService.GetByIdAsync(Guid.Empty, CancellationToken.None)
		);
	}

	[TestMethod]
	public async Task GetByIdAsync_DoctorFound_ReturnsDoctor()
	{
		var d = new Doctor();
		_uowMock.Setup(
			u => u.DoctorRepository.GetByIdAsync(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => d);

		var res = await _doctorService.GetByIdAsync(Guid.Empty, CancellationToken.None);

		Assert.IsTrue(ReferenceEquals(d, res));
	}

	#endregion

	#region CreateAsync

	[TestMethod]
	public async Task CreateAsync_RoomNotFound_ThrowsRoomNotFoundException()
	{
		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<RoomNotFoundException>(
			() => _doctorService.CreateAsync(null!, null!, null, 0, 0, null, CancellationToken.None)
		);
	}

	[TestMethod]
	public async Task CreateAsync_RoomFound_SpecialityNotFound_ThrowsSpecialityNotFoundException()
	{
		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<SpecialityNotFoundException>(
			() => _doctorService.CreateAsync(null!, null!, null, 0, 0, null, CancellationToken.None)
		);
	}

	[TestMethod]
	public async Task
		CreateAsync_RoomFound_SpecialityFound_DistrictIsNotNull_DistrictNotFound_ThrowsDistrictNotFoundException()
	{
		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Speciality());

		_uowMock.Setup(
			u => u.DistrictRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<DistrictNotFoundException>(
			() => _doctorService.CreateAsync(null!, null!, null, 0, 0, 1, CancellationToken.None)
		);
	}


	[TestMethod]
	public async Task CreateAsync_RoomFound_SpecialityFound_DistrictIsNull_Checked_ReturnsDoctor()
	{
		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Speciality());


		_uowMock.Setup(u => u.DoctorRepository.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
		        .ReturnsAsync((Doctor d, CancellationToken _) => d);

		var d = await _doctorService.CreateAsync("s", "n", null, 0, 0, null, CancellationToken.None);
		Assert.AreEqual("n", d.Name);
		Assert.AreEqual("s", d.Surname);
		Assert.AreEqual(null, d.Patronymic);
		Assert.AreEqual(0, d.RoomId);
		Assert.AreEqual(0, d.SpecialityId);
		Assert.AreEqual(null, d.DistrictId);
		_uowMock.Verify(u => u.SaveChangesAsync());
	}

	[TestMethod]
	public async Task CreateAsync_RoomFound_SpecialityFound_DistrictIsNotNull_Checked_ReturnsDoctor()
	{
		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Speciality());

		_uowMock.Setup(
			u => u.DistrictRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new District());


		_uowMock.Setup(u => u.DoctorRepository.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
		        .ReturnsAsync((Doctor d, CancellationToken _) => d);

		var d = await _doctorService.CreateAsync("s", "n", null, 0, 0, 1, CancellationToken.None);
		Assert.AreEqual("n", d.Name);
		Assert.AreEqual("s", d.Surname);
		Assert.AreEqual(null, d.Patronymic);
		Assert.AreEqual(0, d.RoomId);
		Assert.AreEqual(0, d.SpecialityId);
		Assert.AreEqual(1, d.DistrictId);
		_uowMock.Verify(u => u.SaveChangesAsync());
	}

	#endregion

	#region Edit

	[TestMethod]
	public async Task EditAsync_DoctorNotFound_ThrowsDoctorNotFoundException()
	{
		_uowMock.Setup(u => u.DoctorRepository.GetAll())
		        .Returns(Array.Empty<Doctor>().AsQueryable());

		await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
			() => _doctorService.EditAsync(new Doctor(), CancellationToken.None)
		);
	}

	[TestMethod]
	public async Task EditAsync_RoomNotFound_ThrowsRoomNotFoundException()
	{
		_uowMock.Setup(u => u.DoctorRepository.GetAll())
		        .Returns(new[] {new Doctor()}.AsQueryable());

		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<RoomNotFoundException>(
			() => _doctorService.EditAsync(new Doctor(), CancellationToken.None)
		);
	}

	[TestMethod]
	public async Task EditAsync_RoomFound_SpecialityNotFound_ThrowsSpecialityNotFoundException()
	{
		_uowMock.Setup(u => u.DoctorRepository.GetAll())
		        .Returns(new[] {new Doctor()}.AsQueryable());

		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<SpecialityNotFoundException>(
			() => _doctorService.EditAsync(new Doctor(), CancellationToken.None)
		);
	}

	[TestMethod]
	public async Task
		EditAsync_RoomFound_SpecialityFound_DistrictIsNotNull_DistrictNotFound_ThrowsDistrictNotFoundException()
	{
		_uowMock.Setup(u => u.DoctorRepository.GetAll())
		        .Returns(new[] {new Doctor()}.AsQueryable());

		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Speciality());

		_uowMock.Setup(
			u => u.DistrictRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<DistrictNotFoundException>(
			() => _doctorService.EditAsync(new Doctor() {DistrictId = 1}, CancellationToken.None)
		);
	}


	[TestMethod]
	public async Task EditAsync_DistrictIsNull_Checked_ReturnsDoctor()
	{
		var arr = new[] {new Doctor()};
		_uowMock.Setup(u => u.DoctorRepository.GetAll())
		        .Returns(arr.AsQueryable());

		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Speciality());

		_uowMock.Setup(u => u.DoctorRepository.Update(It.IsAny<Doctor>()))
		        .Callback((Doctor d) => arr[0] = d);


		var d = await _doctorService.EditAsync(
			new Doctor()
			{
				Name = "n",
				Surname = "s",
				Patronymic = "p",
				RoomId = 1,
				SpecialityId = 1,
				DistrictId = null
			},
			CancellationToken.None
		);

		Assert.IsTrue(ReferenceEquals(d, arr[0]));
		Assert.AreEqual("n", arr[0].Name);
		Assert.AreEqual("s", arr[0].Surname);
		Assert.AreEqual("p", arr[0].Patronymic);
		Assert.AreEqual(1, arr[0].RoomId);
		Assert.AreEqual(1, arr[0].SpecialityId);
		Assert.AreEqual(null, arr[0].DistrictId);
		_uowMock.Verify(u => u.SaveChangesAsync());

	}

	[TestMethod]
	public async Task EditAsync_DistrictIsNotNull_Checked_ReturnsDoctor()
	{
		var arr = new[] {new Doctor()};
		_uowMock.Setup(u => u.DoctorRepository.GetAll())
		        .Returns(arr.AsQueryable());

		_uowMock.Setup(u => u.DoctorRepository.GetAll())
		        .Returns(new[] {new Doctor()}.AsQueryable());

		_uowMock.Setup(
			u => u.RoomRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Room());

		_uowMock.Setup(
			u => u.SpecialityRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new Speciality());

		_uowMock.Setup(
			u => u.DistrictRepository.GetByIdAsync(
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => new District());


		_uowMock.Setup(u => u.DoctorRepository.Update(It.IsAny<Doctor>()))
		        .Callback((Doctor d) => arr[0] = d);


		var d = await _doctorService.EditAsync(
			new Doctor()
			{
				Name = "n",
				Surname = "s",
				Patronymic = "p",
				RoomId = 1,
				SpecialityId = 1,
				DistrictId = 1
			},
			CancellationToken.None
		);

		Assert.IsTrue(ReferenceEquals(d, arr[0]));
		Assert.AreEqual("n", arr[0].Name);
		Assert.AreEqual("s", arr[0].Surname);
		Assert.AreEqual("p", arr[0].Patronymic);
		Assert.AreEqual(1, arr[0].RoomId);
		Assert.AreEqual(1, arr[0].SpecialityId);
		Assert.AreEqual(1, arr[0].DistrictId);
		_uowMock.Verify(u => u.SaveChangesAsync());

	}

	#endregion

	#region Delete

	[TestMethod]
	public async Task Delete_DoctorNotFound_ThrowsDoctorNotFoundException()
	{
		_uowMock.Setup(
			u => u.DoctorRepository.GetByIdAsync(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => null);

		await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
			() => _doctorService.Delete(Guid.Empty, CancellationToken.None)
		);
	}

	[TestMethod]
	public async Task Delete_DoctorFound_ReturnsRemovedDoctor()
	{
		var d = new Doctor();
		_uowMock.Setup(
			u => u.DoctorRepository.GetByIdAsync(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>()
			)
		).ReturnsAsync(() => d);


		var res = await _doctorService.Delete(Guid.Empty, CancellationToken.None);

		Assert.IsTrue(ReferenceEquals(d, res));
		_uowMock.Verify(u => u.DoctorRepository.Remove(d));
		_uowMock.Verify(u => u.SaveChangesAsync());
	}

	#endregion
}