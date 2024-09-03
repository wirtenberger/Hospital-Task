namespace Hospital.Core.Model;

public class District
{
	public int Id { get; set; }

	public int Number { get; set; }

	public static District Create(int id, int number)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number);

		return new District()
		{
			Id = id,
			Number = number
		};
	}
}