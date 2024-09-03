namespace Hospital.Core.Model;

public class Room
{
	public int Id { get; set; }

	public int Number { get; set; }

	public static Room Create(int id, int number)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number);
		return new Room()
		{
			Id = id,
			Number = number
		};
	}
}